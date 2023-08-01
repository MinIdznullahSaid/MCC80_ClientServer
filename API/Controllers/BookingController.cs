using API.Contracts;
using API.DTOs.BookingDtos;
using API.DTOs.RoomDtos;
using API.Models;
using API.Services;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _bookingService.GetAll();
        if (!result.Any())
        {
            return NotFound(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data is not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<BookingDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieve data",
            Data = result
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _bookingService.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data is not found"
            });
        }

            return Ok(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data is found",
                Data = result
            });
        }

    [HttpPost]
    public IActionResult Insert(NewBookingDto newBookingDto)
    {
        var result = _bookingService.Create(newBookingDto);
        if (result is null)
        {
            return StatusCode(500, new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Insert Failed"
            });
        }

        return Ok(new ResponseHandler<BookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Insert Success",
            Data = result
        });
        }

    [HttpPut]
    public IActionResult Update(BookingDto bookingDto)
    {
        var result = _bookingService.Update(bookingDto);

        if (result is -1)
        {
            return NotFound(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        if (result is 0)
        {
            return StatusCode(500, new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Update Failed"
            });
        }

        return Ok(new ResponseHandler<BookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Update Success",
            Data = bookingDto
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var result = _bookingService.Delete(guid);

        if (result is -1)
        {
            return NotFound(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        if (result is 0)
        {
            return StatusCode(500, new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Delete Failed"
            });
        }

        return Ok(new ResponseHandler<BookingDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Delete Success",
        });
    }

    [HttpGet("FreeRoomsToday")]
    public IActionResult FreeRoomsToday()
    {
        var result = _bookingService.FreeRoomsToday();
        if (result is null)
        {
            return NotFound(new ResponseHandler<RoomDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Room not found"
            });
        }

        return Ok(
            new ResponseHandler<IEnumerable<RoomDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Success retrieving data",
                Data = result
            });
    }

    [HttpGet("BookingLength")]
    public IActionResult BookingLength()
    {
        var result = _bookingService.BookingLength();
        if (result is null)
        {
            return NotFound(new ResponseHandler<BookingDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid Is Not Found",
                Data = null
            });
        }
        return Ok(new ResponseHandler<IEnumerable<GetBookingLengthDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Success Retrieved",
            Data = result
        });
    }

    [HttpGet("detail")]
    public IActionResult GetAllBookingDetail()
    {
        var result = _bookingService.GetAllBookingDetail();
        if (!result.Any())
        {
            return NotFound(new ResponseHandler<BookingDetailDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<BookingDetailDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieve data",
            Data = result
        });
    }

    [HttpGet("detail/{guid}")]
    public IActionResult GetAllBookingDetail(Guid guid)
    {
        var result = _bookingService.GetBookingDetailByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseHandler<BookingDetailDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<BookingDetailDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data is found",
            Data = result
        });
    }

}