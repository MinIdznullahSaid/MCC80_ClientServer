using API.Contracts;
using API.DTOs.EducationDtos;
using API.Repositories;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace API.Utilities.Validations.Educations;

public class NewEducationValidator : AbstractValidator<NewEducationDto>
{
   
    public NewEducationValidator()
    {

        RuleFor(e => e.Guid)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(e => e.Major)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(e => e.Degree)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(e => e.GPA)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(e => e.UniversityGuid)
            .NotEmpty().WithMessage("Field must be filled");
    }
}
