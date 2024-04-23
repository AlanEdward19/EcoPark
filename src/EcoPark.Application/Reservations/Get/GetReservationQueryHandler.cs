namespace EcoPark.Application.Reservations.Get;

public class GetReservationQueryHandler(IRepository<ReservationModel> repository) : IHandler<GetReservationQuery, ReservationSimplifiedViewModel?>
{
    public async Task<ReservationSimplifiedViewModel?> HandleAsync(GetReservationQuery command, CancellationToken cancellationToken)
    {
        ReservationSimplifiedViewModel? result = null;
        var reservation =  await repository.GetByIdAsync(command, cancellationToken);

        if (reservation != null)
        {
            if (command.IncludeParkingSpace)
            {
                ParkingSpaceSimplifiedViewModel parkingSpace = new(reservation.ParkingSpace.Floor,
                    reservation.ParkingSpace.ParkingSpaceName, reservation.ParkingSpace.IsOccupied,
                    reservation.ParkingSpace.ParkingSpaceType);

                result = new ReservationViewModel(reservation.CarId, reservation.ClientId,
                    reservation.ReservationCode, reservation.Status, 
                    reservation.ReservationDate, reservation.ExpirationDate,
                    parkingSpace);
            }

            else
                result = new ReservationSimplifiedViewModel(reservation.CarId, reservation.ClientId,
                    reservation.ReservationCode,
                reservation.Status, reservation.ReservationDate, reservation.ExpirationDate);
        }

        return result;
    }
}