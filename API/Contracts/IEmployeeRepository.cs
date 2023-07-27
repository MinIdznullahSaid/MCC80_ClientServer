using API.DTOs;
using API.Models;
using API.Utilities.Handlers;

namespace API.Contracts;

public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    bool IsNotExist(string value);
    string GetLastNIK();
    Employee? GetByEmail(string email);

    Employee? CheckEmail(string email);
}

