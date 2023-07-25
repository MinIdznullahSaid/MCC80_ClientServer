using API.DTOs;
using FluentValidation;

namespace API.Utilities.Validations.Bookings;

public class NewBookingValidator : AbstractValidator<NewBookingDto>
{
    public NewBookingValidator()
    {

        RuleFor(b => b.StartDate)
            .NotEmpty().WithMessage("Field must be filled")
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Input is not valid")
            .LessThan(b => b.EndDate);

        RuleFor(b => b.EndDate)
            .NotEmpty().WithMessage("Field must be filled")
            .GreaterThan(b => b.StartDate).WithMessage("Input is not valid");

        RuleFor(b => b.Status)
            .IsInEnum();

        RuleFor(b => b.Remark)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(b => b.RoomGuid)
            .NotEmpty().WithMessage("Field must be filled");

        RuleFor(b => b.EmployeeGuid)
            .NotEmpty().WithMessage("Field must be filled");
    }
}
