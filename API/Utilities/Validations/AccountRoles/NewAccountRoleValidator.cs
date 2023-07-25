using API.DTOs;
using FluentValidation;

namespace API.Utilities.Validations.AccountRoles;

public class NewAccountRoleValidator : AbstractValidator<NewAccountRoleDto>
{
    public NewAccountRoleValidator()
    {

        RuleFor(ar => ar.AccountGuid)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(ar => ar.RoleGuid)
            .NotEmpty().WithMessage("Field must be filled");
    }
}
