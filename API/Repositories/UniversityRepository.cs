using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(BookingDbContext context) : base(context) { }

    public bool IsNotExist(string value)
    {
        return _context.Set<University>()
                       .SingleOrDefault(e => e.Code.Contains(value)
                       || e.Name.Contains(value)) is null;
    }

    public Guid GetLastUniversityGuid()
    {
        return _context.Set<University>().ToList().LastOrDefault().Guid;
    }
}

