using EcoPark.Application.Rewards.Insert;
using FluentValidation;

namespace EcoPark.Application.Rewards.Validators;

public class InsertRewardCommandValidator : AbstractValidator<InsertRewardCommand>
{
    public InsertRewardCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Name is required");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name can't be empty");

        RuleFor(x => x.Description)
            .NotNull()
            .WithMessage("Description is required");
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description can't be empty");

        RuleFor(x => x.AvailableQuantity)
            .GreaterThanOrEqualTo(1)
            .When(x => x.AvailableQuantity != null)
            .WithMessage("AvailableQuantity must be equal or greater than 1");

        RuleFor(x => x.RequiredPoints)
            .NotNull()
            .WithMessage("RequiredPoints is required");
        RuleFor(x => x.RequiredPoints)
            .GreaterThan(0)
            .WithMessage("RequiredPoints must be greater than 0");

        RuleFor(x => x.IsActive)
            .NotNull()
            .WithMessage("IsActive is required");

        RuleFor(x => x.Url)
            .NotNull()
            .WithMessage("Url is required");
        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage("Url can't be empty");

        RuleFor(x => x.ExpirationDate)
            .Must(x => x.Value > DateTime.Today)
            .When(x => x.ExpirationDate != null)
            .WithMessage("ExpirationDate must be greater than today");

        RuleFor(x => x.LocationId)
            .NotNull()
            .WithMessage("LocationId is required");
    }
}