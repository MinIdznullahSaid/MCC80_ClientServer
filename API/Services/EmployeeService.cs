using API.Contracts;
using API.DTOs.EmployeeDtos;
using API.Models;
using API.Utilities.Handlers;

namespace API.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;

    public EmployeeService(IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository)
    {
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
    }

    public IEnumerable<EmployeeDto> GetAll()
    {
        var employees = _employeeRepository.GetAll();
        if (!employees.Any())
        {
            return Enumerable.Empty<EmployeeDto>(); // employee is null or not found;
        }

        var employeeDtos = new List<EmployeeDto>();
        foreach (var employee in employees)
        {
            employeeDtos.Add((EmployeeDto)employee);
        }

        return employeeDtos; // employee is found;
    }

    public EmployeeDto? GetByGuid(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee is null)
        {
            return null; // employee is null or not found;
        }

        return (EmployeeDto)employee; // employee is found;
    }

    public EmployeeDto? Create(NewEmployeeDto newEmployeeDto)
    {
        Employee newNIK = newEmployeeDto;
        newNIK.NIK = GenerateHandler.GenerateNIK(_employeeRepository.GetLastNIK());
        var employee = _employeeRepository.Create(newNIK);
        if (employee is null)
        {
            return null; // employee is null or not found;
        }

        return (EmployeeDto)employee; // employee is found;
    }

    public int Update(UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = _employeeRepository.GetByGuid(updateEmployeeDto.Guid);
        if (employee is null)
        {
            return -1; // employee is null or not found;
        }

        Employee toUpdate = updateEmployeeDto;
        toUpdate.NIK = employee.NIK;
        toUpdate.CreatedDate = employee.CreatedDate;
        var result = _employeeRepository.Update(toUpdate);

        return result ? 1 // employee is updated;
            : 0; // employee failed to update;
    }

    public int Delete(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee is null)
        {
            return -1; // employee is null or not found;
        }

        var result = _employeeRepository.Delete(employee);

        return result ? 1 // employee is deleted;
            : 0; // employee failed to delete;
    }

    public IEnumerable<EmployeeDetailDto> GetAllEmployeeDetail()
    {
        var employees = _employeeRepository.GetAll();

        if (!employees.Any())
        {
            return Enumerable.Empty<EmployeeDetailDto>();
        }

        var employeesDetailDto = new List<EmployeeDetailDto>();

        foreach (var employee in employees)
        {
            var education = _educationRepository.GetByGuid(employee.Guid);
            var university = _universityRepository.GetByGuid(education.UniversityGuid);

            EmployeeDetailDto employeeDetail = new EmployeeDetailDto
            {
                EmployeeGuid = employee.Guid,
                NIK = employee.NIK,
                FullName = employee.FirstName + " " + employee.LastName,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                HiringDate = employee.HiringDate,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Major = education.Major,
                Degree = education.Degree,
                GPA = education.GPA,
                UniversityName = university.Name
            };

            employeesDetailDto.Add(employeeDetail);
        };

        return employeesDetailDto; // employeeDetail is found;
    }
    public EmployeeDetailDto? GetEmployeeDetailByGuid(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);

        if (employee == null)
        {
            return null;
        }

        var education = _educationRepository.GetByGuid(employee.Guid);
        var university = _universityRepository.GetByGuid(education.UniversityGuid);

        return new EmployeeDetailDto
        {
            EmployeeGuid = employee.Guid,
            NIK = employee.NIK,
            FullName = employee.FirstName + " " + employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Major = education.Major,
            Degree = education.Degree,
            GPA = education.GPA,
            UniversityName = university.Name
        };

    }
}