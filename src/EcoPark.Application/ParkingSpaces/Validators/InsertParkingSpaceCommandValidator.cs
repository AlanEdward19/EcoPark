using FluentValidation;

namespace EcoPark.Application.ParkingSpaces.Validators;

public class InsertParkingSpaceCommandValidator : AbstractValidator<InsertParkingSpaceCommand>
{
    public InsertParkingSpaceCommandValidator()
    {
        RuleFor(x => x.ParkingSpaceName)
            .NotNull()
            .WithMessage("ParkingSpaceName is Required");
        RuleFor(x => x.ParkingSpaceName)
            .NotEmpty()
            .WithMessage("ParkingSpaceName can't be empty");

        RuleFor(x => x.Type)
            .NotNull()
            .WithMessage("Type is required");

        RuleFor(x => x.Floor)
            .NotNull()
            .WithMessage("Floor is required");

        RuleFor(x => x.IsOccupied)
            .NotNull()
            .WithMessage("IsOccupied is required");

        RuleFor(x => x.LocationId)
            .NotNull()
            .WithMessage("LocationId is required");
        RuleFor(x => x.LocationId)
            .Must(x => !x.Equals(Guid.Empty))
            .WithMessage("LocationId can't be empty");
    }
}