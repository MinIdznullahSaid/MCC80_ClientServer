using API.DTOs;
using API.Models;
using API.Utilities.Handlers;

namespace API.Contracts;

public interface IAccountRepository : IGeneralRepository<Account>
{
    bool IsNotExist(string value);
}

