using API.Contracts;
using API.DTOs;
using API.Repositories;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace API.Utilities.Validations.Universities;

public class NewUniversityValidator : AbstractValidator<NewUniversityDto>
{

    public NewUniversityValidator()
    {
        

        RuleFor(u => u.Code)
            .NotEmpty().WithMessage("Field must be filled")
            .MaximumLength(6);

        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("Field must be filled");
    }
}
