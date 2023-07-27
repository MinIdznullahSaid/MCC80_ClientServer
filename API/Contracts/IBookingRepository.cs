using API.DTOs.BookingDtos;
using API.DTOs.RoomDtos;
using API.Models;
using API.Utilities.Enums;

namespace API.Contracts;

public interface IBookingRepository : IGeneralRepository<Booking>
{
    IEnumerable<RoomDto> GetFreeRoomsToday();
    Booking? GetStatus(StatusLevel status);

    DateTime GetStartDate(Guid guid);
    DateTime GetEndDate(Guid guid);
}

