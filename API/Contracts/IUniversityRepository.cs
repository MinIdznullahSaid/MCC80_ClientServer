using API.Models;

namespace API.Contracts;

public interface IUniversityRepository : IGeneralRepository<University>
{
    bool IsNotExist(string value);
    University? GetUniversityByCode(string code);
}

