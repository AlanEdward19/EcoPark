namespace Application.Reservations.Get;

public class GetReservationQueryHandler : IHandler<Guid, Reservation>
{
    public async Task<Reservation> HandleAsync(Guid command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}