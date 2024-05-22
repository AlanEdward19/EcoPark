using FluentValidation;

namespace EcoPark.Application.Reservations.Validators;

public class InsertReservationCommandValidator : AbstractValidator<InsertReservationCommand>
{
    public InsertReservationCommandValidator()
    {
        RuleFor(x => x.CarId)
            .NotNull()
            .WithMessage("CarId is required");
        RuleFor(x => x.CarId)
            .Must(x => !x.Equals(Guid.Empty))
            .WithMessage("CarId can't be empty");

        RuleFor(x => x.ParkingSpaceId)
            .NotNull()
            .WithMessage("ParkingSpaceId is required");
        RuleFor(x => x.ParkingSpaceId)
            .Must(x => !x.Equals(Guid.Empty))
            .WithMessage("ParkingSpaceId can't be empty");

        RuleFor(x => x.ReservationDate)
            .NotNull()
            .WithMessage("ReservationDate is required");
        RuleFor(x => x.ReservationDate)
            .Must(x => x > DateTime.Now)
            .WithMessage("ReservationDate must be in the future");
        RuleFor(x => x.ReservationDate)
            .Must(x => x!.Value.Date == DateTime.Today)
            .WithMessage("ReservationDate must be within the same day");
    }
}