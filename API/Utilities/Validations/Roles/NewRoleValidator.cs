using API.DTOs.RoleDtos;
using FluentValidation;

namespace API.Utilities.Validations.Roles;

public class NewRoleValidator : AbstractValidator<NewRoleDto>
{
    public NewRoleValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Field must be filled");
    }
}
