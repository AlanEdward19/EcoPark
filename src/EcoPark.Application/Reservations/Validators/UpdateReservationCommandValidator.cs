using FluentValidation;

namespace EcoPark.Application.Reservations.Validators;

public class UpdateReservationCommandValidator : AbstractValidator<UpdateReservationCommand>
{
    public UpdateReservationCommandValidator()
    {
        RuleFor(x => x.ReservationDate)
            .Must(x => x > DateTime.Now)
            .WithMessage("ReservationDate must be in the future")
            .When(x => x.ReservationDate != null);

        RuleFor(x => x.ReservationDate)
            .Must(x => x!.Value.Date == DateTime.Today)
            .WithMessage("ReservationDate must be within the same day")
            .When(x => x.ReservationDate != null);
    }
}