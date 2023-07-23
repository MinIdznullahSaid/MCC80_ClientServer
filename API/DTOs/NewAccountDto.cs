using API.Models;
using API.Utilities.Enums;

namespace API.DTOs;

public class NewAccountDto
{
    public Guid Guid { get; set; }
    public string Password { get; set; }
    public bool IsDeleted { get; set; }
    public int OTP { get; set; }
    public bool IsUsed { get; set; }
    public DateTime ExpiredTime { get; set; }

    public static implicit operator Account(NewAccountDto newAccountDto)
    {
        return new Account
        {
            Guid = newAccountDto.Guid,
            Password = newAccountDto.Password,
            IsDeleted = newAccountDto.IsDeleted,
            OTP = newAccountDto.OTP,
            IsUsed = newAccountDto.IsUsed,
            ExpiredTime = newAccountDto.ExpiredTime,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator NewAccountDto(Account account)
    {
        return new NewAccountDto
        {
            Guid = account.Guid,
            Password = account.Password,
            IsDeleted = account.IsDeleted,
            OTP = account.OTP,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime
        };
    }
}