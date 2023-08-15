using API.DTOs.AccountDtos;
using API.DTOs.EmployeeDtos;
using API.Models;
using API.Utilities.Handlers;

namespace Client.Contracts;

public interface IAccountRepository : IGeneralRepository<LoginDto, Guid>
{
    public Task<ResponseHandler<TokenDto>> Login(LoginDto entity);
}
