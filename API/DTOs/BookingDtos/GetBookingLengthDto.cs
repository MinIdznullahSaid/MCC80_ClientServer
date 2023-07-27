using API.DTOs.BookingDtos;

namespace API.Services;

public class GetBookingLengthDto
{
    public Guid RoomGuid { get; set; }
    public string RoomName { get; set; }
    public double BookingLength { get; set; }
}