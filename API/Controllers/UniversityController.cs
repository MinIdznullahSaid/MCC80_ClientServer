using API.Contracts;
using API.DTOs.UniversityDtos;
using API.Models;
using API.Services;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/universities")]
[Authorize]
public class UniversityController : ControllerBase
{
    private readonly UniversityService _universityService;

    public UniversityController(UniversityService universityService)
    {
        _universityService = universityService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _universityService.GetAll();
        if (!result.Any())
        {
            return NotFound(new ResponseHandler<UniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data is not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<UniversityDto>>
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
        var result = _universityService.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseHandler<UniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data is not found"
            });
        }

        return Ok(new ResponseHandler<UniversityDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Insert Success",
            Data = result
        });
    }

    [HttpPost]
    public IActionResult Insert(NewUniversityDto newUniversityDto)
    {
        var result = _universityService.Create(newUniversityDto);
        if (result is null)
        {
            return StatusCode(500,new ResponseHandler<UniversityDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Insert Failed"
            });
        }

        return Ok(new ResponseHandler<UniversityDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Insert Success",
            Data = result
        });
    }

    [HttpPut]
    public IActionResult Update(UniversityDto universityDto)
    {
        var result = _universityService.Update(universityDto);

        if (result is -1)
        {
            return NotFound(new ResponseHandler<UniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        if (result is 0)
        {
            return StatusCode(500, new ResponseHandler<UniversityDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Update Failed"
            });
        }

        return Ok(new ResponseHandler<UniversityDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Update Success",
            Data = universityDto
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var result = _universityService.Delete(guid);

        if (result is -1)
        {
            return NotFound(new ResponseHandler<UniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        if (result is 0)
        {
            return StatusCode(500, new ResponseHandler<UniversityDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Delete Failed"
            });
        }

        return Ok(new ResponseHandler<UniversityDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Delete Success",
        });
    }
}