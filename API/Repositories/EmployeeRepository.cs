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

    public string GetLastNIK()
    {
        var lastEmployee = _context.Set<Employee>()
                .OrderByDescending(e => e.NIK)
                .FirstOrDefault().NIK;

        return lastEmployee;
    }

    public Employee? GetByEmail(string email)
    {
        return _context.Set<Employee>().SingleOrDefault(e => e.Email.Contains(email));
    }

    public Employee? CheckEmail(string email)
    {
        return _context.Set<Employee>().FirstOrDefault(e => e.Email == email);
    }
}

