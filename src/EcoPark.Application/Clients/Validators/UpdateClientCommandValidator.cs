using EcoPark.Application.Clients.Update;
using EcoPark.Application.Utils;
using FluentValidation;

namespace EcoPark.Application.Clients.Validators;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email must be a valid email address")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Password)
            .Must(ValidatorUtils.ValidPassword)
            .WithMessage("Password must contain at least 8 characters, a number, an uppercase letter, a lowercase letter, and a special character")
            .When(x => !string.IsNullOrWhiteSpace(x.Password));
    }
}