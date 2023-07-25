using API.Contracts;
using API.Data;
using API.DTOs;
using API.Models;

namespace API.Repositories;

public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingDbContext context) : base(context) { }

    public bool IsNotExist(string value)
    {
        return _context.Set<Employee>()
                       .SingleOrDefault(e => e.Email.Contains(value)
                       || e.PhoneNumber.Contains(value)) is null;
    }

    public object GetLastNIK(EmployeeDto EmployeeDto)
    {
        return _context.Employees
                .OrderByDescending(e => e.NIK)
                .FirstOrDefault();
    }
}

