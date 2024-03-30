namespace EcoPark.Application.Reservations.List;

public class ListReservationsQueryHandler : IHandler<IEnumerable<Guid>?, IEnumerable<ReservationModel>>
{
    public async Task<IEnumerable<ReservationModel>> HandleAsync(IEnumerable<Guid>? command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}