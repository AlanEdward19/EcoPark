using FluentValidation;

namespace EcoPark.Application.Locations.Validators;

public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
{
    public UpdateLocationCommandValidator()
    {
        RuleFor(x => x.ReservationGraceInMinutes)
            .Must(x => x >= 0)
            .WithMessage("Reservation Grace In Minutes must be equal or greater than 0")
            .When(x => x.ReservationGraceInMinutes != null);

        RuleFor(x => x.CancellationFeeRate)
            .Must(x => x >= 0)
            .WithMessage("Cancellation Fee Rate must be equal or greater than 0")
            .When(x => x.CancellationFeeRate != null);

        RuleFor(x => x.ReservationFeeRate)
            .Must(x => x >= 0)
            .WithMessage("Reservation Fee Rate must be equal or greater than 0")
            .When(x => x.ReservationFeeRate != null);

        RuleFor(x => x.HourlyParkingRate)
            .Must(x => x >= 0)
            .WithMessage("Hourly Parking Rate must be equal or greater than 0")
            .When(x => x.HourlyParkingRate != null);
    }
}