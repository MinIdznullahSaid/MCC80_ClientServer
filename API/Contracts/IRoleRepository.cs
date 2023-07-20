using API.Models;

namespace API.Contracts;

public interface IRoleRepository
{
    IEnumerable<Role> GetAll();
    Role? GetByGuid(Guid id);
    Role? Create(Role role);
    bool Update(Role role);
    bool Delete(Role role);
}

