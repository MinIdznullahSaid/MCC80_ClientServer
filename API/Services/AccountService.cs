using API.Contracts;
using API.Data;
using API.DTOs.AccountDtos;
using API.DTOs.AccountRoleDtos;
using API.DTOs.EducationDtos;
using API.DTOs.EmployeeDtos;
using API.DTOs.UniversityDtos;
using API.Models;
using API.Repositories;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;
using TokenHandler = Microsoft.IdentityModel.Tokens.TokenHandler;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IEmailHandler _emailHandler;
    private readonly ITokenHandler _tokenHandler;
    private readonly BookingDbContext _dbContext;


    public AccountService(
        IAccountRepository accountRepository,
        IEmployeeRepository employeeRepository,
        IEducationRepository educationRepository,
        IUniversityRepository universityRepository,
        BookingDbContext dbContext,
        ITokenHandler tokenHandler,
        IEmailHandler emailHandler,
        IAccountRoleRepository accountRoleRepository)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
        _tokenHandler = tokenHandler;
        _dbContext = dbContext;
        _emailHandler = emailHandler;
        _accountRoleRepository = accountRoleRepository;
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
        var createAccount = newAccountDto;
        createAccount.Password = HashingHandler.GenerateHash(newAccountDto.Password);
        var account = _accountRepository.Create(createAccount); 
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
        toUpdate.Password = HashingHandler.GenerateHash(accountDto.Password);
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

    public string Login(LoginDto loginDto)
    {
        var employeeAccount = (from e in _employeeRepository.GetAll()
                               join a in _accountRepository.GetAll() on e.Guid equals a.Guid
                               where e.Email == loginDto.Email
                               select new LoginDto()
                               {
                                   Email = e.Email,
                                   Password = a.Password
                               }).FirstOrDefault();

        if (employeeAccount is null)
        {
            return "0"; // Email or Password incorrect.
        }

        var getEmployee = _employeeRepository.GetByEmail(loginDto.Email);
        var getRoles = _accountRoleRepository.GetRoleNamesByAccountGuid(getEmployee.Guid);

        /*if (getEmployee is null)
        {
            return "-1"; // Employee not found
        }*/


            var claims = new List<Claim> 
            {
            new Claim("Guid", getEmployee.Guid.ToString()),
            new Claim("FullName", $"{getEmployee.FirstName}{getEmployee.LastName}"),
            new Claim("Email", getEmployee.Email)
            };

            foreach (var role in getRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var generatedToken = _tokenHandler.GenerateToken(claims);
            if (generatedToken is null)
            {
                return "-1";
            }

            return generatedToken;
        }


    public int Register(RegisterDto registerDto)
    {
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
                Password = HashingHandler.GenerateHash(registerDto.Password),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ExpiredTime = DateTime.Now
            });

            var accountRole = _accountRoleRepository.Create(new NewAccountRoleDto
            {
                AccountGuid = account.Guid,
                RoleGuid = Guid.Parse("f96a6e22-f9b7-46a4-1890-08db922b6b7e")
            });

            transaction.Commit();
            return 1;
        }
        catch
        {
            transaction.Rollback();
            return -1;
        }
        
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
        {
            return -1;
        }

        _emailHandler.SendEmail(forgotPasswordDto.Email,
                                "Booking - Forgot Password",
                                $"Your OTP is {otp}");

        return 1;
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
            Password = HashingHandler.GenerateHash(changePasswordDto.NewPassword)
        };

        var isUpdated = _accountRepository.Update(account);
        if (!isUpdated)
        {
            return -4; //Account Not Update
        }

        return 1;
    }
}

    

