namespace API.Contracts;

public interface IGeneralRepository<Entity>
{
    IEnumerable<Entity> GetAll();
    Entity? GetByGuid(Guid guid);
    Entity? Create(Entity entity);
    bool Update(Entity entity);
    bool Delete(Entity entity);
}