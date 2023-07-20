using API.Models;

namespace API.Contracts;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetAll();
    Employee? GetByGuid(Guid id);
    Employee? Create(Employee employee);
    bool Update(Employee employee);
    bool Delete(Employee employee);
}

