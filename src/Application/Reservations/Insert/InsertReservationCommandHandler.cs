namespace Application.Reservations.Insert;

public class InsertReservationCommandHandler : IHandler<InsertReservationCommand, Guid>
{
    public async Task<Guid> HandleAsync(InsertReservationCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}