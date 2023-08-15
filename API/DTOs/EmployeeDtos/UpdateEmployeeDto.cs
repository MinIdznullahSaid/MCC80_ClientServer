using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.EmployeeDtos;

public class UpdateEmployeeDto
{
    public Guid Guid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public GenderLevel Gender { get; set; }
    public DateTime HiringDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public static implicit operator Employee(UpdateEmployeeDto updateEmployeeDto)
    {
        return new Employee
        {
            Guid = updateEmployeeDto.Guid,
            FirstName = updateEmployeeDto.FirstName,
            LastName = updateEmployeeDto.LastName,
            BirthDate = updateEmployeeDto.BirthDate,
            Gender = updateEmployeeDto.Gender,
            HiringDate = updateEmployeeDto.HiringDate,
            Email = updateEmployeeDto.Email,
            PhoneNumber = updateEmployeeDto.PhoneNumber,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator UpdateEmployeeDto(Employee employee)
    {
        return new UpdateEmployeeDto
        {
            Guid = employee.Guid,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber
        };
    }
}