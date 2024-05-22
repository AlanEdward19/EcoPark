using EcoPark.Application.Cars.Update;
using EcoPark.Application.Utils;
using FluentValidation;

namespace EcoPark.Application.Cars.Validators;

public class UpdateCarCommandValidator : AbstractValidator<UpdateCarCommand>
{
    public UpdateCarCommandValidator()
    {
        RuleFor(x => x.Year)
            .Must(x => x >= 1900 && x <= DateTime.Now.Year)
            .WithMessage("Year must be between 1900 and current year")
            .When(x => x.Year != null);

        RuleFor(x => x.Plate)
            .Must(ValidatorUtils.ValidateLicensePlate)
            .WithMessage("Place must be in a valid format")
            .When(x => !string.IsNullOrWhiteSpace(x.Plate));

        RuleFor(x => x)
            .Must(x => ValidatorUtils.IsValidTypeAndFuelCombination(x.Type!.Value, x.FuelType!.Value))
            .WithMessage("Invalid Type and FuelType combination")
            .When(x => x.FuelType != null && x.FuelConsumptionPerLiter != null);
    }
}