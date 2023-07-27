using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class AccountRepository : GeneralRepository<Account>, IAccountRepository
{
    public AccountRepository(BookingDbContext context) : base(context) { }

    public bool IsNotExist(string value)
    {
        return _context.Set<Employee>()
                       .SingleOrDefault(e => e.Email.Contains(value)
                       || e.PhoneNumber.Contains(value)) is null;
    }

    public Employee? GetByEmail(string email)
    {
        return _context.Set<Employee>().SingleOrDefault(e => e.Email.Contains(email));
    }
}

