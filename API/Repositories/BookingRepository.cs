using API.Contracts;
using API.Data;
using API.DTOs.BookingDtos;
using API.DTOs.RoomDtos;
using API.Models;
using API.Utilities.Enums;

namespace API.Repositories;

public class BookingRepository : GeneralRepository<Booking>, IBookingRepository
{
    private readonly IRoomRepository _roomRepository;
    public BookingRepository(BookingDbContext context) : base(context) { }

    public DateTime GetEndDate(Guid guid)
    {
        var data = _context.Set<Booking>().Where(booking => booking.RoomGuid == guid).SingleOrDefault().EndDate;
        return data;
    }

    public IEnumerable<RoomDto> GetFreeRoomsToday()
    {
        throw new NotImplementedException();
    }

    public DateTime GetStartDate(Guid guid)
    {
        var data = _context.Set<Booking>().Where(booking => booking.RoomGuid == guid).SingleOrDefault().StartDate;
        return data;
    }

    public Booking? GetStatus(StatusLevel status)
    {
        return _context.Set<Booking>().Find(status);
    }
}

