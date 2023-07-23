using API.Contracts;
using API.DTOs;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/roles")]
public class RoleController : ControllerBase
{
    private readonly RoleService _roleService;

    public RoleController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _roleService.GetAll();
        if (!result.Any())
        {
            return NotFound("No data found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _roleService.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Guid is not found");
        }

        return Ok(result);
    }

    [HttpPost]
    public IActionResult Insert(NewRoleDto newRoleDto)
    {
        var result = _roleService.Create(newRoleDto);
        if (result is null)
        {
            return StatusCode(500, "Error Retrieve from database");
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(RoleDto roleDto)
    {
        var result = _roleService.Update(roleDto);

        if (result is -1)
        {
            return NotFound("Guid is not found");
        }

        if (result is 0)
        {
            return StatusCode(500, "Error Retrieve from database");
        }

        return Ok("Update success");
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var result = _roleService.Delete(guid);

        if (result is -1)
        {
            return NotFound("Guid is not found");
        }

        if (result is 0)
        {
            return StatusCode(500, "Error Retrieve from database");
        }

        return Ok("Delete success");
    }
}