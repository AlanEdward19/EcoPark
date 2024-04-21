namespace EcoPark.Application.Reservations.List;

public class ListReservationsQueryHandler(IRepository<ReservationModel> repository) : IHandler<ListReservationQuery, IEnumerable<ReservationSimplifiedViewModel>>
{
    public async Task<IEnumerable<ReservationSimplifiedViewModel>> HandleAsync(ListReservationQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}