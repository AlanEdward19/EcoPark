namespace EcoPark.Application.ParkingSpaces.List;

public class ListParkingSpacesQueryHandler(DatabaseDbContext databaseDbContext)
    : IHandler<ListParkingSpacesQuery, IEnumerable<ParkingSpaceSimplifiedViewModel>?>
{
    public async Task<IEnumerable<ParkingSpaceSimplifiedViewModel>?> HandleAsync(ListParkingSpacesQuery command, CancellationToken cancellationToken)
    {
        bool hasParkingIds = command.ParkingSpaceIds != null && command.ParkingSpaceIds.Any();
        IQueryable<ParkingSpaceModel> query = databaseDbContext.ParkingSpaces.Include(ps => ps.Location);

        IEnumerable<ParkingSpaceModel> parkingSpaceModels;

        if (hasParkingIds)
            query = query.Where(ps => command.ParkingSpaceIds!.Contains(ps.Id));

        if (command.IncludeReservations!.Value)
        {
            List<ParkingSpaceViewModel>? result = hasParkingIds
                ? new(command.ParkingSpaceIds!.Count())
                : new(100);

            query = query.Include(ps => ps.Reservations);

            parkingSpaceModels = await query.ToListAsync(cancellationToken);

            foreach (var parkingSpaceModel in parkingSpaceModels)
            {
                IEnumerable<ReservationSimplifiedViewModel>? reservations = parkingSpaceModel.Reservations?.Select(x =>
                    new ReservationSimplifiedViewModel(x.CardId, x.ClientId, x.ReservationCode, x.Status,
                        x.ReservationDate, x.ExpirationDate));

                LocationSimplifiedViewModel location = new(parkingSpaceModel.Location.Name,
                    parkingSpaceModel.Location.Address);

                ParkingSpaceViewModel parkingSpace = new(parkingSpaceModel.Floor,
                    parkingSpaceModel.ParkingSpaceName,
                    parkingSpaceModel.IsOccupied, parkingSpaceModel.ParkingSpaceType, location, reservations);

                result.Add(parkingSpace);
            }

            return result;

        }
        else
        {
            List<ParkingSpaceSimplifiedViewModel>? result = hasParkingIds
                ? new(command.ParkingSpaceIds!.Count())
                : new(100);

            parkingSpaceModels = await query.ToListAsync(cancellationToken);

            foreach (var parkingSpaceModel in parkingSpaceModels)
            {
                LocationSimplifiedViewModel location = new(parkingSpaceModel.Location.Name,
                    parkingSpaceModel.Location.Address);

                ParkingSpaceSimplifiedViewModel parkingSpace = new(parkingSpaceModel.Floor,
                    parkingSpaceModel.ParkingSpaceName,
                    parkingSpaceModel.IsOccupied, parkingSpaceModel.ParkingSpaceType, location);

                result.Add(parkingSpace);
            }

            return result;
        }
    }
}