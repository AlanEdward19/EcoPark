using FluentValidation;
using EcoPark.Application.Utils;

namespace EcoPark.Application.Employees.Validators;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email must a valid email address")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Password)
            .Must(ValidatorUtils.ValidPassword)
            .WithMessage("Password must contain at least 8 characters, a number, an uppercase letter, a lowercase letter, and a special character")
            .When(x => !string.IsNullOrWhiteSpace(x.Password));
    }
}