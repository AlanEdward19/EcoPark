using EcoPark.Application.CarbonEmission.Insert;
using FluentValidation;

namespace EcoPark.Application.CarbonEmission.Validators;

public class InsertCarbonEmissionCommandValidator : AbstractValidator<InsertCarbonEmissionCommand>
{
    public InsertCarbonEmissionCommandValidator()
    {
        RuleFor(x => x.Emission)
            .NotNull()
            .WithMessage("Emission is required");

        RuleFor(x => x.Emission)
            .GreaterThan(0)
            .WithMessage("Emission must be greater than 0");

        RuleFor(x => x.Forecast)
            .NotNull()
            .WithMessage("Forecast is required");

        RuleFor(x => x.Forecast)
            .GreaterThan(0)
            .WithMessage("Forecast must be greater than 0");

        RuleFor(x => x.Inhibition)
            .NotNull()
            .WithMessage("Inhibition is required");

        RuleFor(x => x.Inhibition)
            .GreaterThan(0)
            .WithMessage("Inhibition must be greater than 0");
    }
}