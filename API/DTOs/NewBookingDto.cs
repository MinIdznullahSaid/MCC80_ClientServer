using API.Models;
using API.Utilities.Enums;

namespace API.DTOs;

public class NewBookingDto
{
    public Guid Guid { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public StatusLevel Status { get; set; }
    public string Remark { get; set; }
    public Guid RoomGuid { get; set; }
    public Guid EmployeeGuid { get; set; }

    public static implicit operator Booking(NewBookingDto newBookingDto)
    {
        return new Booking
        {
            Guid = newBookingDto.Guid,
            StartDate = newBookingDto.StartDate,
            EndDate = newBookingDto.EndDate,
            Status = newBookingDto.Status,
            Remark = newBookingDto.Remark,
            RoomGuid = newBookingDto.RoomGuid,
            EmployeeGuid = newBookingDto.EmployeeGuid,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator NewBookingDto(Booking booking)
    {
        return new NewBookingDto
        {
            Guid = booking.Guid,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status,
            Remark = booking.Remark,
            RoomGuid = booking.RoomGuid,
            EmployeeGuid = booking.EmployeeGuid
        };
    }
}