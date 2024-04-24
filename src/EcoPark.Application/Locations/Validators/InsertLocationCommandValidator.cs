using FluentValidation;

namespace EcoPark.Application.Locations.Validators;

public class InsertLocationCommandValidator : AbstractValidator<InsertLocationCommand>
{
    public InsertLocationCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Name is Required");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name can't be empty");

        RuleFor(x => x.Address)
            .NotNull()
            .WithMessage("Address is Required");
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address can't be empty");

        RuleFor(x => x.ReservationGraceInMinutes)
            .NotNull()
            .WithMessage("Reservation Grace In Minutes is Required");
        RuleFor(x => x.ReservationGraceInMinutes)
            .Must(x => x >= 0)
            .WithMessage("Reservation Grace In Minutes must be equal or greater than 0");

        RuleFor(x => x.CancellationFeeRate)
            .NotNull()
            .WithMessage("Cancellation Fee Rate is Required");
        RuleFor(x => x.CancellationFeeRate)
            .Must(x => x >= 0)
            .WithMessage("Cancellation Fee Rate must be equal or greater than 0");

        RuleFor(x => x.ReservationFeeRate)
            .NotNull()
            .WithMessage("Reservation Fee Rate is Required");
        RuleFor(x => x.ReservationFeeRate)
            .Must(x => x >= 0)
            .WithMessage("Reservation Fee Rate must be equal or greater than 0");

        RuleFor(x => x.HourlyParkingRate)
            .NotNull()
            .WithMessage("Hourly Parking Rate is Required");
        RuleFor(x => x.HourlyParkingRate)
            .Must(x => x >= 0)
            .WithMessage("Hourly Parking Rate must be equal or greater than 0");
    }
}