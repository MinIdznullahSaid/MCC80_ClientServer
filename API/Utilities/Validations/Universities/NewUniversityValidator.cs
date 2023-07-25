using API.Contracts;
using API.DTOs;
using API.Repositories;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace API.Utilities.Validations.Universities;

public class NewUniversityValidator : AbstractValidator<NewUniversityDto>
{
    private readonly IUniversityRepository _universityRepository;

    public NewUniversityValidator(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;

        RuleFor(u => u.Code)
            .NotEmpty().WithMessage("Field must be filled")
            .MaximumLength(6)
            .Must(IsDuplicateValue).WithMessage("Code already exists");

        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("Field must be filled")
            .Must(IsDuplicateValue).WithMessage("University name already exists");
    }

    private bool IsDuplicateValue(string arg)
    {
        return _universityRepository.IsNotExist(arg);
    }
}
