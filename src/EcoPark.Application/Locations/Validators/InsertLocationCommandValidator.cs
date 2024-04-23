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

        RuleFor(x => x.ReservationGraceInMinutes).NotNull().WithMessage("Reservation Grace In Minutes is Required");
        RuleFor(x => x.CancellationFeeRate).NotNull().WithMessage("Cancellation Fee Rate is Required");
        RuleFor(x => x.ReservationFeeRate).NotNull().WithMessage("Reservation Fee Rate is Required");
        RuleFor(x => x.HourlyParkingRate).NotNull().WithMessage("Hourly Parking Rate is Required");
    }
}