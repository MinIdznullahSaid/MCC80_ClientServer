using API.Contracts;
using API.DTOs.AccountDtos;
using FluentValidation;

namespace API.Utilities.Validations.Accounts;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
{
    private readonly IEmployeeRepository _employeeRepository;

    public ChangePasswordValidator(IEmployeeRepository employeeRepository)
    {
        RuleFor(a => a.Email)
               .NotEmpty().WithMessage("Email is required")
               .EmailAddress().WithMessage("Email is not valid");

        RuleFor(a => a.NewPassword)
                    .NotEmpty()
                    .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$").WithMessage("Password is not valid");

        RuleFor(r => r.ConfirmPassword)
           .NotEmpty().WithMessage("Field must be filled")
           .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$").WithMessage("Password is not valid")
           .Equal(r => r.NewPassword).WithMessage("Confirmed password is wrong");

        RuleFor(a => a.OTP)
                    .NotEmpty();
    }
    
}
