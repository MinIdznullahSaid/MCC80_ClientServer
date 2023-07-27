﻿using API.Contracts;
using API.DTOs.AccountDtos;
using FluentValidation;

namespace API.Utilities.Validations.Accounts;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    public ForgotPasswordValidator(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;

        RuleFor(e => e.Email)
            .EmailAddress().WithMessage("Email is not valid")
            .NotEmpty().WithMessage("Email is required");
    }
}
