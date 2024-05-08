using EcoPark.Application.Rewards.Update;
using FluentValidation;

namespace EcoPark.Application.Rewards.Validators;

public class UpdateRewardCommandValidator : AbstractValidator<UpdateRewardCommand>
{
    public UpdateRewardCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id is required");

        RuleFor(x => x.AvailableQuantity)
            .GreaterThanOrEqualTo(1)
            .When(x => x.AvailableQuantity != null)
            .WithMessage("AvailableQuantity must be equal or greater than 1");

        RuleFor(x => x.RequiredPoints)
            .GreaterThan(0)
            .When(x => x.RequiredPoints != null)
            .WithMessage("RequiredPoints must be greater than 0");
    }
}