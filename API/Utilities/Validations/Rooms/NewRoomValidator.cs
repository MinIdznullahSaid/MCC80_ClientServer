using API.DTOs;
using FluentValidation;

namespace API.Utilities.Validations.Rooms;

public class NewRoomValidator : AbstractValidator<NewRoomDto>
{
    public NewRoomValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(r => r.Floor)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(r => r.Capacity)
            .NotEmpty().WithMessage("Field must be filled");
    }
}
