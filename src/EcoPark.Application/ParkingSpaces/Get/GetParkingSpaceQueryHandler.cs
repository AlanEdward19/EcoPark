namespace EcoPark.Application.ParkingSpaces.Get;

public class GetParkingSpaceQueryHandler(DatabaseDbContext databaseContext) : IHandler<GetParkingSpaceQuery, ParkingSpaceSimplifiedViewModel?>
{
    public async Task<ParkingSpaceSimplifiedViewModel?> HandleAsync(GetParkingSpaceQuery command,
        CancellationToken cancellationToken)
    {
        ParkingSpaceModel? parkingSpaceModel;
        ParkingSpaceSimplifiedViewModel? result = null;

        IQueryable<ParkingSpaceModel> query = databaseContext.ParkingSpaces.Include(ps => ps.Location);

        if (command.IncludeReservations)
        {
            query = query.Include(ps => ps.Reservations);

            parkingSpaceModel =
                await query.FirstOrDefaultAsync(ps => ps.Id == command.ParkingSpaceId, cancellationToken);

            if (parkingSpaceModel != null)
            {
                LocationSimplifiedViewModel location = new(parkingSpaceModel.Location.Name,
                    parkingSpaceModel.Location.Address);

                IEnumerable<ReservationSimplifiedViewModel>? reservations = parkingSpaceModel.Reservations?.Select(x =>
                    new ReservationSimplifiedViewModel(x.CardId, x.ClientId, x.ReservationCode, x.Status,
                        x.ReservationDate, x.ExpirationDate));

                result = new ParkingSpaceViewModel(parkingSpaceModel.Floor, parkingSpaceModel.ParkingSpaceName,
                    parkingSpaceModel.IsOccupied, parkingSpaceModel.ParkingSpaceType, location, reservations);
            }
        }
        else
        {
            parkingSpaceModel =
                await query.FirstOrDefaultAsync(ps => ps.Id == command.ParkingSpaceId, cancellationToken);

            if (parkingSpaceModel != null)
            {
                LocationSimplifiedViewModel location = new(parkingSpaceModel.Location.Name,
                    parkingSpaceModel.Location.Address);

                result = new ParkingSpaceSimplifiedViewModel(parkingSpaceModel.Floor,
                    parkingSpaceModel.ParkingSpaceName, parkingSpaceModel.IsOccupied,
                    parkingSpaceModel.ParkingSpaceType, location);
            }
        }

        return result;
    }
}