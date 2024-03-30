namespace EcoPark.Application.Reservations.Get;

public class GetReservationQueryHandler(DatabaseDbContext databaseContext) : IHandler<GetReservationQuery, ReservationSimplifiedViewModel?>
{
    public async Task<ReservationSimplifiedViewModel?> HandleAsync(GetReservationQuery command, CancellationToken cancellationToken)
    {
        ReservationModel? reservationModel;
        ReservationSimplifiedViewModel? result = null;

        if (command.IncludeParkingSpace)
        {
            reservationModel = await databaseContext.Reservations
                .Include(r => r.ParkingSpace).ThenInclude(parkingSpaceModel => parkingSpaceModel.Location)
                .FirstOrDefaultAsync(r => r.Id == command.ReservationId, cancellationToken);

            if (reservationModel != null)
            {
                LocationSimplifiedViewModel location = new(reservationModel.ParkingSpace.Location.Name,
                    reservationModel.ParkingSpace.Location.Address);

                ParkingSpaceSimplifiedViewModel parkingSpace = new(reservationModel.ParkingSpace.Floor,
                    reservationModel.ParkingSpace.ParkingSpaceName, reservationModel.ParkingSpace.IsOccupied,
                    reservationModel.ParkingSpace.ParkingSpaceType, location);

                result = new ReservationViewModel(reservationModel.CardId, reservationModel.ClientId,
                    reservationModel.ReservationCode,
                    reservationModel.Status, reservationModel.ReservationDate, reservationModel.ExpirationDate,
                    parkingSpace);
            }
        }
        else
        {
            reservationModel = await databaseContext.Reservations
                .FirstOrDefaultAsync(r => r.Id == command.ReservationId, cancellationToken);

            if (reservationModel != null)
                result = new ReservationSimplifiedViewModel(reservationModel.CardId, reservationModel.ClientId,
                    reservationModel.ReservationCode,
                    reservationModel.Status, reservationModel.ReservationDate, reservationModel.ExpirationDate);
        }

        return result;
    }
}