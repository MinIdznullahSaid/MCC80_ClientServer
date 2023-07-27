using API.Contracts;
using API.DTOs.BookingDtos;
using API.DTOs.RoomDtos;
using API.Models;
using API.Repositories;
using API.Utilities.Enums;

namespace API.Services;

public class BookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;

    public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
    }

    public IEnumerable<BookingDto> GetAll()
    {
        var bookings = _bookingRepository.GetAll();
        if (!bookings.Any())
        {
            return Enumerable.Empty<BookingDto>(); // booking is null or not found;
        }

        var bookingDtos = new List<BookingDto>();
        foreach (var booking in bookings)
        {
            bookingDtos.Add((BookingDto)booking);
        }

        return bookingDtos; // booking is found;
    }

    public BookingDto? GetByGuid(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);
        if (booking is null)
        {
            return null; // booking is null or not found;
        }

        return (BookingDto)booking; // booking is found;
    }

    public BookingDto? Create(NewBookingDto newBookingDto)
    {
        var booking = _bookingRepository.Create(newBookingDto);
        if (booking is null)
        {
            return null; // booking is null or not found;
        }

        return (BookingDto)booking; // booking is found;
    }

    public int Update(BookingDto bookingDto)
    {
        var booking = _bookingRepository.GetByGuid(bookingDto.Guid);
        if (booking is null)
        {
            return -1; // booking is null or not found;
        }

        Booking toUpdate = bookingDto;
        toUpdate.CreatedDate = booking.CreatedDate;
        var result = _bookingRepository.Update(toUpdate);

        return result ? 1 // booking is updated;
            : 0; // booking failed to update;
    }

    public int Delete(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);
        if (booking is null)
        {
            return -1; // booking is null or not found;
        }

        var result = _bookingRepository.Delete(booking);

        return result ? 1 // booking is deleted;
            : 0; // booking failed to delete;
    }

    public IEnumerable<RoomDto> FreeRoomsToday()
    {
        List<RoomDto> roomDtos = new List<RoomDto>();
        var bookings = GetAll();
        var freeBookings = bookings.Where(b => b.Status == StatusLevel.Done);
        var freeBookingsToday = freeBookings.Where(b =>
    b.EndDate < DateTime.Now);
        foreach (var booking in freeBookingsToday)
        {
            var roomGuid = booking.RoomGuid;
            var room = _roomRepository.GetByGuid(roomGuid);
            RoomDto roomDto = new RoomDto()
            {
                Guid = roomGuid,
                Capacity = room.Capacity,
                Floor = room.Floor,
                Name = room.Name
            };
            roomDtos.Add(roomDto);
        }
        if (!roomDtos.Any())
        {
            return null; // No free room today
        }

        return roomDtos; // free room today 
    }

    public IEnumerable<GetBookingLengthDto> BookingLength()
    {
        var bookinglist = new List<GetBookingLengthDto>();
        var getbooking = _bookingRepository.GetAll();
        TimeSpan length = new TimeSpan();
        TimeSpan Start = new TimeSpan(09, 00, 00);
        TimeSpan End = new TimeSpan(17, 00, 00);
        TimeSpan OneDay = new TimeSpan(08, 00, 00);
        if (getbooking == null)
        {
            return null;
        }
        foreach (var bookings in getbooking)
        {
            TimeSpan bookinglength = new TimeSpan();
            var startdate = bookings.StartDate;
            var enddate = bookings.EndDate;
            while (startdate < enddate)
            {
                if (startdate.DayOfWeek != DayOfWeek.Sunday && startdate.DayOfWeek != DayOfWeek.Saturday)
                {
                    if (startdate.TimeOfDay >= Start && enddate.TimeOfDay <= End)
                    {
                        if (startdate.TimeOfDay == enddate.TimeOfDay && startdate.Date < enddate.Date)
                        {
                            bookinglength += OneDay;
                        }
                        else
                        {
                            length = enddate.TimeOfDay - startdate.TimeOfDay;
                            bookinglength += length;
                        }
                    }
                    else
                    {
                        bookinglength += OneDay;
                    }
                    startdate = startdate.AddDays(1);
                }
                else
                {
                    startdate = startdate.AddDays(1);
                }
            }
            var room = _roomRepository.GetByGuid(bookings.RoomGuid);
            var bookinglengthdto = new GetBookingLengthDto()
            {
                RoomGuid = bookings.RoomGuid,
                RoomName = room.Name,
                BookingLength = bookinglength.TotalHours
            };
            bookinglist.Add(bookinglengthdto);
        }
        return bookinglist;
    }

}

