using API.DTOs.AccountDtos;
using FluentValidation;

namespace API.Utilities.Validations.Accounts;

public class NewAccountValidator : AbstractValidator<NewAccountDto>
{
    public NewAccountValidator()
    {

        RuleFor(a => a.Guid)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(a => a.Password)
            .NotEmpty().WithMessage("Field must be filled")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$").WithMessage("Password is not valid");

        RuleFor(a => a.IsDeleted)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(a => a.OTP)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(a => a.IsUsed)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(a => a.ExpiredTime)
            .NotEmpty().WithMessage("Field must be filled")
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Input is not valid");
    }
}
