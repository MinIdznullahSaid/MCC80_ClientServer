using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class AccountRoleRepository : IAccountRoleRepository
{
    private readonly BookingDbContext _context;

    public AccountRoleRepository(BookingDbContext context)
    {
        _context = context;
    }

    public IEnumerable<AccountRole> GetAll()
    {
        return _context.Set<AccountRole>()
                       .ToList();
    }

    public AccountRole? GetByGuid(Guid guid)
    {
        var data = _context.Set<AccountRole>()
                           .Find(guid);
        _context.ChangeTracker.Clear();
        return data;
    }

    public AccountRole? Create(AccountRole accountRole)
    {
        try
        {
            _context.Set<AccountRole>()
                    .Add(accountRole);
            _context.SaveChanges();
            return accountRole;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(AccountRole accountRole)
    {
        try
        {
            _context.Entry(accountRole)
                    .State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(AccountRole accountRole)
    {
        try
        {
            _context.Set<AccountRole>()
                    .Remove(accountRole);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}

