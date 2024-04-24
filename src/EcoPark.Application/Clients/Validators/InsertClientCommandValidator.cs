using EcoPark.Application.Clients.Insert;
using EcoPark.Application.Utils;
using FluentValidation;

namespace EcoPark.Application.Clients.Validators;

public class InsertClientCommandValidator : AbstractValidator<InsertClientCommand>
{
    public InsertClientCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage("Email is Required");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email can't be empty");
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email must be a valid email address");

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
            .WithMessage("Password must contain at least 8 characters, a number, an uppercase letter, a lowercase letter, and a special character");
    }
}