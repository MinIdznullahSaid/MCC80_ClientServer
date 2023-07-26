using API.Models;
using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.AccountRoleDtos;

public class AccountRoleDto
{
    public Guid Guid { get; set; }
    public Guid AccountGuid { get; set; }
    public Guid RoleGuid { get; set; }

    public static implicit operator AccountRole(AccountRoleDto accountRoleDto)
    {
        return new AccountRole
        {
            Guid = accountRoleDto.Guid,
            AccountGuid = accountRoleDto.AccountGuid,
            RoleGuid = accountRoleDto.RoleGuid,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator AccountRoleDto(AccountRole accountRole)
    {
        return new AccountRoleDto
        {
            Guid = accountRole.Guid,
            AccountGuid = accountRole.AccountGuid,
            RoleGuid = accountRole.RoleGuid
        };
    }
}