using API.Contracts;
using API.DTOs.AccountDtos;
using API.DTOs.EducationDtos;
using API.DTOs.EmployeeDtos;
using API.DTOs.UniversityDtos;
using API.Models;
using API.Repositories;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;

    public AccountService(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;

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

        Employee employeeToCreate = new NewEmployeeDto
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
            ExpiredTime = DateTime.Now.AddYears(1),
            OTP = 000,
            Password = registerDto.Password,
        };

        var accountResult = _accountRepository.Create(account);

        if (employeeResult is null || universityResult is null || educationResult is null || accountResult is null)
        {
            return 0;
        }

        return 1;
        /*try
        {

            var employee = new Employee
            {
                Guid = new Guid(),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                BirthDate = registerDto.BirthDate,
                HiringDate = registerDto.HiringDate,
                Gender = registerDto.Gender,
            };
            var university = new University
            {
                Name = registerDto.UniversityName
            };
            var education = new Education
            {
                Degree = registerDto.Degree,
                Major = registerDto.Major,
                GPA = registerDto.GPA
            };
            var account = new Account
            {
                Password = registerDto.Password,
            };
            
            employee.Account = account;
            education.University = university;
            education.Employee = employee;

            var createEmployee = _employeeRepository.Create(employee);
            var createUniversity = _universityRepository.Create(university);
            var createEducation = _educationRepository.Create(education);
            var createAccount = _accountRepository.Create(account);

            return 1; // register success

        }
        catch
        {
            return 0; // register failed
        }
    }*/
    }
}