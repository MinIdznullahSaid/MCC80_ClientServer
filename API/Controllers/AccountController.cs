using API.Contracts;
using API.DTOs.AccountDtos;
using API.Models;
using API.Services;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _accountService.GetAll();
        if (!result.Any())
        {
            return NotFound(new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data is not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<AccountDto>>
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
        var result = _accountService.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data is not found"
            });
        }

        return Ok(new ResponseHandler<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data is found",
            Data = result
        });
    }

    [HttpPost]
    public IActionResult Insert(NewAccountDto newAccountDto)
    {
        var result = _accountService.Create(newAccountDto);
        if (result is null)
        {
            return StatusCode(500, new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Insert Failed"
            });
        }

            return Ok(new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Insert Success",
                Data = result
            });
        }

    [HttpPut]
    public IActionResult Update(AccountDto accountDto)
    {
        var result = _accountService.Update(accountDto);

        if (result is -1)
        {
            return NotFound(new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        if (result is 0)
        {
            return StatusCode(500, new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Update Failed"
            });
        }

        return Ok(new ResponseHandler<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Update Success",
            Data = accountDto
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var result = _accountService.Delete(guid);

        if (result is -1)
        {
            return NotFound(new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        if (result is 0)
        {
            return StatusCode(500, new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Delete Failed"
            });
        }

        return Ok(new ResponseHandler<AccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Delete Success",
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login(LoginDto loginDto)
    {
        var result = _accountService.Login(loginDto);

        if (result is "0")
        {
            return StatusCode(500, new ResponseHandler<AccountDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Delete Failed"
            });
        }

        if (result is "-1")
        {
            return NotFound(new ResponseHandler<LoginDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Email or Password is incorrect"
            });
        }

        if (result is "-2")
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<LoginDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error when generate token"
            });
        }

        return Ok(new ResponseHandler<TokenDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Login Success",
            Data = new TokenDto {
                Token = result
            }
        });
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public IActionResult Register(RegisterDto registerDto)
    {
        var result = _accountService.Register(registerDto);

        if (result is 0)
        {
            return StatusCode(500, new ResponseHandler<RegisterDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Something is Wrong, Register Failed"
            });
        }

        return Ok(new ResponseHandler<RegisterDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Register Success",
            Data = registerDto
        });
    }

    [HttpPost("forgotpassword")]
    [AllowAnonymous]
    public IActionResult ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var isUpdate = _accountService.ForgotPassword(forgotPasswordDto);
        if (isUpdate is 0)
            return NotFound(new ResponseHandler<ForgotPasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Email is not found"
            });

        if (isUpdate is -1)
        {
            return StatusCode(500, new ResponseHandler<ForgotPasswordDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from database"
            });
        }

        return Ok(new ResponseHandler<ForgotPasswordDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "OTP has been sent to your email",
            Data = forgotPasswordDto
        });
    }

    [HttpPost("changepassword")]
    [AllowAnonymous]

    public IActionResult ChangePassword(ChangePasswordDto changePassword)
    {
        var update = _accountService.ChangePassword(changePassword);
        if (update is 0)
        {
            return NotFound(new ResponseHandler<ChangePasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Email is not found"
            });
        }

        if (update is -1)
        {
            return NotFound(new ResponseHandler<ChangePasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "OTP is incorrect"
            });
        }

        if (update is -2)
        {
            return NotFound(new ResponseHandler<ChangePasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "OTP was used"
            });
        }

        if (update is -3)
        {
            return NotFound(new ResponseHandler<ChangePasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "OTP was expired"
            });
        }

        if (update is -4)
        {
            return NotFound(new ResponseHandler<ChangePasswordDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Update Failed"
            });
        }

        return Ok(new ResponseHandler<ChangePasswordDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Update password success",
        });
    }
}