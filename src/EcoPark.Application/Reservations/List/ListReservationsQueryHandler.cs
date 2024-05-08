namespace EcoPark.Application.Reservations.List;

public class ListReservationsQueryHandler(IRepository<ReservationModel> repository) : IHandler<ListReservationQuery, IEnumerable<ReservationSimplifiedViewModel>>
{
    public async Task<IEnumerable<ReservationSimplifiedViewModel>> HandleAsync(ListReservationQuery command, CancellationToken cancellationToken)
    {
        var reservations = await repository.ListAsync(command, cancellationToken);

        if (reservations == null || !reservations.Any())
            return Enumerable.Empty<ReservationSimplifiedViewModel>();

        if (command.IncludeParkingSpace)
        {
            List<ReservationViewModel> result = new(reservations.Count());

            foreach (var reservationModel in reservations)
            {
                var parkingSpace = reservationModel.ParkingSpace;
                ParkingSpaceSimplifiedViewModel parkingSpaceViewModel = new(parkingSpace.Id, parkingSpace.Floor,
                    parkingSpace.ParkingSpaceName, parkingSpace.IsOccupied, parkingSpace.ParkingSpaceType);

                ReservationViewModel reservation = new(reservationModel.Id, reservationModel.CarId,
                    reservationModel.ClientId, reservationModel.ReservationCode, reservationModel.Punctuation, reservationModel.Status, reservationModel.ReservationDate,
                    reservationModel.ExpirationDate, parkingSpaceViewModel);

                result.Add(reservation);
            }

            return result;
        }
        else
        {
            List<ReservationSimplifiedViewModel> result = new(reservations.Count());

            foreach (var reservationModel in reservations)
            {
                ReservationSimplifiedViewModel reservation = new(reservationModel.Id, reservationModel.CarId,
                    reservationModel.ClientId, reservationModel.ReservationCode, reservationModel.Punctuation, reservationModel.Status, reservationModel.ReservationDate,
                    reservationModel.ExpirationDate);

                result.Add(reservation);
            }

            return result;
        }
    }
}