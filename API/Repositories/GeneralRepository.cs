using API.Contracts;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class GeneralRepository<Entity> : IGeneralRepository<Entity>
    where Entity : class
{
    protected readonly BookingDbContext _context;

    public GeneralRepository(BookingDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Entity> GetAll()
    {
        return _context.Set<Entity>()
                       .ToList();
    }

    public Entity? GetByGuid(Guid guid)
    {
        var entity = _context.Set<Entity>()
                           .Find(guid);
        _context.ChangeTracker.Clear();
        return entity;
    }

    public Entity? Create(Entity entity)
    {
        try
        {
            _context.Set<Entity>()
                    .Add(entity);
            _context.SaveChanges();
            return entity;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(Entity entity)
    {
        try
        {
            _context.Entry(entity)
                    .State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(Entity entity)
    {
        try
        {
            _context.Set<Entity>()
                    .Remove(entity);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Clear()
    {
        _context.ChangeTracker.Clear();
    }
}