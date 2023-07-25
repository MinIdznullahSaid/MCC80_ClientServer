using API.DTOs;
using API.Models;

namespace API.Contracts;

public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    object GetLastNIK(EmployeeDto EmployeeDto);
    bool IsNotExist(string value);
}

