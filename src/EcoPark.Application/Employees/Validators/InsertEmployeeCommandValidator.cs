using FluentValidation;
using EcoPark.Application.Utils;

namespace EcoPark.Application.Employees.Validators;

public class InsertEmployeeCommandValidator : AbstractValidator<InsertEmployeeCommand>
{
    public InsertEmployeeCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage("Email is Required");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email can't be empty");
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email must a valid email address");

        RuleFor(x => x.FirstName)
            .NotNull()
            .WithMessage("FirstName is Required");
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("FirstName can't be empty");

        RuleFor(x => x.LastName)
            .NotNull()
            .WithMessage("LastName is Required");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("LastName can't be empty");

        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage("Password is Required");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password can't be empty");
        RuleFor(x => x.Password)
            .Must(ValidatorUtils.ValidPassword)
            .WithMessage(
            "Password must contain at least 8 characters, a number, an uppercase letter, a lowercase letter, and a special character");

        RuleFor(x => x.UserType)
            .NotNull()
            .WithMessage("UserType is Required");

        RuleFor(x => x.UserType)
            .Must(x => x != EUserType.Client)
            .WithMessage("A User can't be created in this route");
    }
}