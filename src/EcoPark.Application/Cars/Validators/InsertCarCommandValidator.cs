using EcoPark.Application.Cars.Insert;
using EcoPark.Application.Utils;
using FluentValidation;

namespace EcoPark.Application.Cars.Validators;

public class InsertCarCommandValidator : AbstractValidator<InsertCarCommand>
{
    public InsertCarCommandValidator()
    {
        RuleFor(x => x.Brand)
            .NotNull()
            .WithMessage("Brand is Required");
        RuleFor(x => x.Brand)
            .NotEmpty()
            .WithMessage("Brand can't be empty");

        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Model is Required");
        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Model can't be empty");

        RuleFor(x => x.Year)
            .NotNull()
            .WithMessage("Year is Required");
        RuleFor(x => x.Year)
            .Must(x => x >= 1900 && x <= DateTime.Now.Year)
            .WithMessage("Year must be between 1900 and current year");

        RuleFor(x => x.Plate)
            .NotNull()
            .WithMessage("Plate is Required");
        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("Plate can't be empty");
        RuleFor(x => x.Plate)
            .Must(ValidatorUtils.ValidateLicensePlate)
            .WithMessage("Place must be in a valid format");

        RuleFor(x => x.Color)
            .NotNull()
            .WithMessage("Color is Required");
        RuleFor(x => x.Color)
            .NotEmpty()
            .WithMessage("Color can't be empty");

        RuleFor(x => x.Type)
            .NotNull()
            .WithMessage("Type is Required");
    }
}