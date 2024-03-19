namespace Application.Reservations.List;

public class ListReservationsQueryHandler : IHandler<IEnumerable<Guid>?, IEnumerable<Reservation>>
{
    public async Task<IEnumerable<Reservation>> HandleAsync(IEnumerable<Guid>? command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}