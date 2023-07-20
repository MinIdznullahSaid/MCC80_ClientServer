using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/account-roles")]
public class AccountRoleController : ControllerBase
{
    private readonly IAccountRoleRepository _accountRoleRepository;

    public AccountRoleController(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _accountRoleRepository.GetAll();
        if (!result.Any())
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _accountRoleRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public IActionResult Insert(AccountRole accountRole)
    {
        var result = _accountRoleRepository.Create(accountRole);
        if (result is null)
        {
            return StatusCode(500, "Error Retrieve from database");
        }

        return Ok(result);
    }

    [HttpPut]
    public IActionResult Update(AccountRole accountRole)
    {
        var check = _accountRoleRepository.GetByGuid(accountRole.Guid);
        if (check is null)
        {
            return NotFound("Guid is not found");
        }

        var result = _accountRoleRepository.Update(accountRole);
        if (!result)
        {
            return StatusCode(500, "Error Retrieve from database");
        }

        return Ok("Update success");
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var data = _accountRoleRepository.GetByGuid(guid);
        if (data is null)
        {
            return NotFound("Guid is not found");
        }

        var result = _accountRoleRepository.Delete(data);
        if (!result)
        {
            return StatusCode(500, "Error Retrieve from database");
        }

        return Ok("Delete success");
    }

}