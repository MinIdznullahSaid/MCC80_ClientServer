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
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRoomRepository _roomRepository;

    public BookingService(IBookingRepository bookingRepository, IEmployeeRepository employeeRepository, IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _employeeRepository = employeeRepository;
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

        _bookingRepository.Clear();

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
        var result = from room in _roomRepository.GetAll()
                     join booking in _bookingRepository.GetAll() on room.Guid equals booking.RoomGuid into books
                     from freeroom in books
                     where (freeroom.EndDate < DateTime.Now)
                     select new RoomDto
                     {
                         Guid = room.Guid,
                         Capacity = room.Capacity,
                         Floor = room.Floor,
                         Name = room.Name
                     };
        var roomresult = result.DistinctBy(room => room.Guid);
        return roomresult;
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

    public IEnumerable<BookingDetailDto> GetAllBookingDetail()
    {
        var bookingsDetail = (from booking in _bookingRepository.GetAll()
                              join employee in _employeeRepository.GetAll() on booking.EmployeeGuid equals employee.Guid
                              join room in _roomRepository.GetAll() on booking.RoomGuid equals room.Guid
                              select new BookingDetailDto
                              {
                                  BookingGuid = booking.Guid,
                                  BookedByNIK = employee.NIK,
                                  BookedBy = employee.FirstName + " " + employee.LastName,
                                  RoomName = room.Name,
                                  StartDate = booking.StartDate,
                                  EndDate = booking.EndDate,
                                  Remark = booking.Remark,
                                  Status = booking.Status
                              });

        if (!bookingsDetail.Any() || bookingsDetail is null)
        {
            return Enumerable.Empty<BookingDetailDto>();
        }

        return bookingsDetail;
    }

    public BookingDetailDto? GetBookingDetailByGuid(Guid guid)
    {
        var allBookings = GetAllBookingDetail();
        return allBookings.FirstOrDefault(b => b.BookingGuid == guid);
    }

}

