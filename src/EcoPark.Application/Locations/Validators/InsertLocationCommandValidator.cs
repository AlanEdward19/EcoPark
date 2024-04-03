using FluentValidation;

namespace EcoPark.Application.Locations.Validators;

public class InsertLocationCommandValidator : AbstractValidator<InsertLocationCommand>
{
    public InsertLocationCommandValidator()
    {
        RuleFor(x => x.Name).NotNull().WithMessage("Name is Required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name can't be empty");

        RuleFor(x => x.Address).NotNull().WithMessage("Address is Required");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address can't be empty");
    }
}