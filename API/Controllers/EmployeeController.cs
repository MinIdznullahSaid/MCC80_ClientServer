using API.Contracts;
using API.DTOs.EmployeeDtos;
using API.Models;
using API.Services;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _employeeService;

    public EmployeeController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _employeeService.GetAll();
        if (!result.Any())
        {
            return NotFound(new ResponseHandler<EmployeeDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data is not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<EmployeeDto>>
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
        var result = _employeeService.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseHandler<EmployeeDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not available"
            });
        }

        return Ok(new ResponseHandler<EmployeeDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data is found",
            Data = result
        });
    }

    [HttpPost]
    public IActionResult Insert(NewEmployeeDto newEmployeeDto)
    {
        var result = _employeeService.Create(newEmployeeDto);
        if (result is null)
        {
            return StatusCode(500, new ResponseHandler<EmployeeDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Insert Failed"
            });
        }

        return Ok(new ResponseHandler<EmployeeDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data is found",
            Data = result
        });
    }

    [HttpPut]
    public IActionResult Update(EmployeeDto employeeDto)
    {
        var result = _employeeService.Update(employeeDto);

        if (result is -1)
        {
            return NotFound(new ResponseHandler<EmployeeDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not available"
            });
        }

        if (result is 0)
        {
            return StatusCode(500, new ResponseHandler<EmployeeDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Update Failed"
            });
        }

        return Ok(new ResponseHandler<EmployeeDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Update Success",
            Data = employeeDto
        });
    }

    [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var result = _employeeService.Delete(guid);

            if (result is -1)
            {
                return NotFound(new ResponseHandler<EmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Guid is not available"
                });
            }

            if (result is 0)
            {
                return StatusCode(500, new ResponseHandler<EmployeeDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Delete Failed"
                });
            }

            return Ok(new ResponseHandler<EmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Delete Success"
            });
        }

    [HttpGet("employees-detail")]
    public IActionResult GetAllEmployeeDetail()
    {
        var result = _employeeService.GetAllEmployeeDetail();
        if (!result.Any())
        {
            return NotFound(new ResponseHandler<EmployeeDetailDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data is not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<EmployeeDetailDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieve data",
            Data = result
        });
    }

    [HttpGet("employees-detail/{guid}")]
    public IActionResult GetEmployeeDetailByGuid(Guid guid)
    {
        var result = _employeeService.GetEmployeeDetailByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseHandler<EmployeeDetailDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not available"
            });
        }

        return Ok(new ResponseHandler<EmployeeDetailDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data is found",
            Data = result
        });
    }

}