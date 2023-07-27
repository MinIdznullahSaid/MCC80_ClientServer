using API.Contracts;
using API.DTOs;
using API.DTOs.AccountDtos;
using FluentValidation;

namespace API.Utilities.Validations.Accounts;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    public RegisterValidator(IEmployeeRepository employeeRepository)
    
    {
        _employeeRepository = employeeRepository;

        RuleFor(r => r.FirstName)
           .NotEmpty().WithMessage("Field must be filled");

        RuleFor(r => r.BirthDate)
            .NotEmpty().WithMessage("Field must be filled")
            .LessThanOrEqualTo(DateTime.Now.AddYears(-10));

        RuleFor(r => r.Gender)
            .IsInEnum();

        RuleFor(r => r.HiringDate)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(r => r.PhoneNumber)
            .NotEmpty().WithMessage("Field must be filled")
            .MaximumLength(20)
            .Matches(@"^\+(?:[0-9]\s?){6,14}[0-9]$").WithMessage("Phone number is not valid")
            .Must(IsDuplicateValue).WithMessage("Phone Number already exists");

        RuleFor(r => r.UniversityCode)
           .NotEmpty().WithMessage("Field must be filled")
           .MaximumLength(6)
           .Must(IsDuplicateValue).WithMessage("Code already exists");

        RuleFor(r => r.UniversityName)
            .NotEmpty().WithMessage("Field must be filled")
            .Must(IsDuplicateValue).WithMessage("University name already exists");

        RuleFor(r => r.Major)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(r => r.Degree)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(r => r.GPA)
            .NotEmpty().WithMessage("Field must be filled")
            .InclusiveBetween(0.0f, 4.0f).WithMessage("GPA harus berada di antara 0.0 hingga 4.0.");

        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Field must be filled")
            .EmailAddress().WithMessage("Email is not valid")
            .Must(IsDuplicateValue).WithMessage("Email already exists");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Field must be filled")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$").WithMessage("Password is not valid");


        RuleFor(r => r.PasswordConfirmed)
            .NotEmpty().WithMessage("Field must be filled")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$").WithMessage("Password is not valid")
            .Equal(r => r.Password).WithMessage("Confirmed password is wrong");


    }

    private bool IsDuplicateValue(string arg)
    {
        return _employeeRepository.IsNotExist(arg);
    }
}