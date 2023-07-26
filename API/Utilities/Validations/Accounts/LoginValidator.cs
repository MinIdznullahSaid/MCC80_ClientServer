using API.DTOs.AccountDtos;
using FluentValidation;

namespace API.Utilities.Validations.Accounts;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {

        RuleFor(a => a.Password)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(a => a.Email)
            .NotEmpty().WithMessage("Field must be filled");

    }
}
