using API.Contracts;
using API.DTOs.EmployeeDtos;
using API.Repositories;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace API.Utilities.Validations.Employees;

public class NewEmployeeValidator : AbstractValidator<NewEmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;

    public NewEmployeeValidator(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;

        RuleFor(e => e.FirstName)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(e => e.BirthDate)
            .NotEmpty().WithMessage("Field must be filled")
            .LessThanOrEqualTo(DateTime.Now.AddYears(-10));

        RuleFor(e => e.Gender)
            .IsInEnum();

        RuleFor(e => e.HiringDate)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(e => e.Email)
            .NotEmpty().WithMessage("Field must be filled")
            .EmailAddress().WithMessage("Email is not valid")
            .Must(IsDuplicateValue).WithMessage("Email already exists");

        RuleFor(e => e.PhoneNumber)
            .NotEmpty().WithMessage("Field must be filled")
            .MaximumLength(20)
            .Matches(@"^\+(?:[0-9]\s?){6,14}[0-9]$").WithMessage("Phone number is not valid")
            .Must(IsDuplicateValue).WithMessage("Phone Number already exists");
    }

    private bool IsDuplicateValue(string arg)
    {
        return _employeeRepository.IsNotExist(arg);
    }
}
