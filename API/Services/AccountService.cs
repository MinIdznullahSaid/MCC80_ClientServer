using API.Contracts;
using API.Data;
using API.DTOs.AccountDtos;
using API.DTOs.EducationDtos;
using API.DTOs.EmployeeDtos;
using API.DTOs.UniversityDtos;
using API.Models;
using API.Repositories;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly BookingDbContext _dbContext;


    public AccountService(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository, BookingDbContext dbContext)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
        _dbContext = dbContext;

    }

    public int ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var getAccountDetail = (from e in _employeeRepository.GetAll()
                                join a in _accountRepository.GetAll() on e.Guid equals a.Guid
                                where e.Email == changePasswordDto.Email
                                select a).FirstOrDefault();
        _accountRepository.Clear();

        if (getAccountDetail is null)
        {
            return 0; // Account not found
        }

        if (getAccountDetail.OTP != changePasswordDto.OTP)
        {
            return -1;
        }

        if (getAccountDetail.IsUsed)
        {
            return -2;
        }

        if (getAccountDetail.ExpiredTime < DateTime.Now)
        {
            return -3;
        }

        var account = new Account
        {
            Guid = getAccountDetail.Guid,
            IsUsed = true,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccountDetail.CreatedDate,
            OTP = getAccountDetail.OTP,
            ExpiredTime = getAccountDetail.ExpiredTime,
            Password = changePasswordDto.NewPassword
        };

        var isUpdated = _accountRepository.Update(account);
        if (!isUpdated)
        {
            return -4; //Account Not Update
        }

        return 1;
    }
    public int ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {

        var getAccountDetail = (from e in _employeeRepository.GetAll()
                                join a in _accountRepository.GetAll() on e.Guid equals a.Guid
                                where e.Email == forgotPasswordDto.Email
                                select a).FirstOrDefault();

        if (getAccountDetail is null)
        {
            return 0;
        }

        _accountRepository.Clear();

        var otp = new Random().Next(000000, 999999);
        var account = new Account
        {
            Guid = getAccountDetail.Guid,
            Password = getAccountDetail.Password,
            ExpiredTime = DateTime.Now.AddMinutes(5),
            OTP = otp,
            IsUsed = false,
            CreatedDate = getAccountDetail.CreatedDate,
            ModifiedDate = DateTime.Now
        };



        var isUpdated = _accountRepository.Update(account);

        if (!isUpdated)
            return -1;

        return 1;
    }

    public IEnumerable<AccountDto> GetAll()
    {
        var accounts = _accountRepository.GetAll();
        if (!accounts.Any())
        {
            return Enumerable.Empty<AccountDto>(); // account is null or not found;
        }

        var accountDtos = new List<AccountDto>();
        foreach (var account in accounts)
        {
            accountDtos.Add((AccountDto)account);
        }

        return accountDtos; // account is found;
    }

    public AccountDto? GetByGuid(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null)
        {
            return null; // account is null or not found;
        }

        return (AccountDto)account; // account is found;
    }

    public AccountDto? Create(NewAccountDto newAccountDto)
    {
        var account = _accountRepository.Create(newAccountDto);
        if (account is null)
        {
            return null; // account is null or not found;
        }

        return (AccountDto)account; // account is found;
    }

    public int Update(AccountDto accountDto)
    {
        var account = _accountRepository.GetByGuid(accountDto.Guid);
        if (account is null)
        {
            return -1; // account is null or not found;
        }

        Account toUpdate = accountDto;
        toUpdate.CreatedDate = account.CreatedDate;
        var result = _accountRepository.Update(toUpdate);

        return result ? 1 // account is updated;
            : 0; // account failed to update;
    }

    public int Delete(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null)
        {
            return -1; // account is null or not found;
        }

        var result = _accountRepository.Delete(account);

        return result ? 1 // account is deleted;
            : 0; // account failed to delete;
    }

    public int Login(LoginDto loginDto)
    {
        var getEmployee = _employeeRepository.GetByEmail(loginDto.Email);

        if (getEmployee is null)
        {
            return 0; // Employee not found
        }

        var getAccount = _accountRepository.GetByGuid(getEmployee.Guid);

        if (getAccount.Password == loginDto.Password)
        {
            return 1; // Login success
        }

        return 0;
    }

    public int Register(RegisterDto registerDto)
    {
        // ini untuk cek emaik sama phone number udah ada atau belum
        if (!_employeeRepository.IsNotExist(registerDto.Email) || !_employeeRepository.IsNotExist(registerDto.PhoneNumber))
        {
            return 0;
        }

        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            var university = _universityRepository.GetUniversityByCode(registerDto.UniversityCode);
            if (university is null)
            {
                var createUniversity = _universityRepository.Create(new University
                {
                    Code = registerDto.UniversityCode,
                    Name = registerDto.UniversityName
                });

                university = createUniversity;
            }

            var newNIK = GenerateHandler.GenerateNIK(_employeeRepository.GetLastNIK());
            var employeeGuid = Guid.NewGuid();

            var employee = _employeeRepository.Create(new Employee
            {
                Guid = employeeGuid,
                NIK = newNIK,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                BirthDate = registerDto.BirthDate,
                Gender = registerDto.Gender,
                HiringDate = registerDto.HiringDate,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber
            });


            var education = _educationRepository.Create(new Education
            {
                Guid = employeeGuid,
                Major = registerDto.Major,
                Degree = registerDto.Degree,
                GPA = registerDto.GPA,
                UniversityGuid = university.Guid
            });

            var account = _accountRepository.Create(new Account
            {
                Guid = employeeGuid,
                OTP = 111,
                IsUsed = true,
                Password = registerDto.Password
            });
            transaction.Commit();
            return 1;
        }
        catch
        {
            transaction.Rollback();
            return -1;
        }

        /* public int RegisterDto? Register(RegisterDto registerDto)
         {
             using var transaction = _dbContext.Database.BeginTransaction();

             try
             {
                 var universityExist = _universityRepository.GetUniversityByCode(registerDto.UniversityCode);
                 var universityToCreate = new University();

                 if (universityExist is null)
                 {
                     universityToCreate.Guid = Guid.NewGuid();
                     universityToCreate.Code = registerDto.UniversityCode;
                     universityToCreate.Name = registerDto.UniversityName;
                     universityToCreate.CreatedDate = DateTime.Now;
                     universityToCreate.ModifiedDate = DateTime.Now;
                 }
                 else
                 {
                     universityToCreate = universityExist;
                 }
                 //University Create
                 var universityResult = _universityRepository.Create(universityToCreate);

                 //Employee Create
                 Employee employeeToCreate = new NewEmployeeDto
                 {
                     FirstName = registerDto.FirstName,
                     LastName = registerDto.LastName,
                     BirthDate = registerDto.BirthDate,
                     Gender = registerDto.Gender,
                     HiringDate = registerDto.HiringDate,
                     Email = registerDto.Email,
                     PhoneNumber = registerDto.PhoneNumber
                 };
                 employeeToCreate.NIK = GenerateHandler.GenerateNIK(_employeeRepository.GetLastNIK());
                 var employeeResult = _employeeRepository.Create(employeeToCreate);

                 //Education Create
                 var educationResult = _educationRepository.Create(new NewEducationDto
                 {
                     Guid = employeeToCreate.Guid,
                     Degree = registerDto.Degree,
                     Major = registerDto.Major,
                     GPA = registerDto.GPA,
                     UniversityGuid = universityResult.Guid
                 });

                 //Account Create
                 var accountResult = _accountRepository.Create(new NewAccountDto
                 {
                     Guid = employeeToCreate.Guid,
                     IsUsed = true,
                     ExpiredTime = DateTime.Now.AddYears(3),
                     OTP = 111,
                     Password = registerDto.Password,
                 });

                 transaction.Commit();
             }
             catch
             {
                 transaction.Rollback();
                 return -1;
             }

             return 1;
         }
         /* Employee employeeToCreate = new NewEmployeeDto
          {
              FirstName = registerDto.FirstName,
              LastName = registerDto.LastName,
              BirthDate = registerDto.BirthDate,
              Gender = registerDto.Gender,
              HiringDate = registerDto.HiringDate,
              Email = registerDto.Email,
              PhoneNumber = registerDto.PhoneNumber,     

      };
          employeeToCreate.NIK = GenerateHandler.GenerateNIK(_employeeRepository.GetLastNIK());
          var employeeResult = _employeeRepository.Create(employeeToCreate);

          University university = new NewUniversityDto
          {
              Code = registerDto.UniversityCode,
              Name = registerDto.UniversityName
          };

          var universityResult = _universityRepository.Create(university);

          Education education = new NewEducationDto
          {
              Guid = employeeToCreate.Guid,
              Degree = registerDto.Degree,
              Major = registerDto.Major,
              GPA = registerDto.GPA,
              UniversityGuid = university.Guid
          };

          var educationResult = _educationRepository.Create(education);

          Account account = new Account
          {
              Guid = employeeToCreate.Guid,
              IsUsed = true,
              ExpiredTime = DateTime.Now.AddYears(2),
              OTP = 000,
              Password = registerDto.Password,
          };

          var accountResult = _accountRepository.Create(account);

          if (employeeResult is null || universityResult is null || educationResult is null || accountResult is null)
          {
              return 0;
          }

          return 1;*/

    }
}

    

