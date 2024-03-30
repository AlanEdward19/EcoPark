namespace EcoPark.Application.Locations.Get;

public class GetLocationQueryHandler(DatabaseDbContext databaseContext) : IHandler<GetLocationQuery, LocationSimplifiedViewModel?>
{
    public async Task<LocationSimplifiedViewModel?> HandleAsync(GetLocationQuery command, CancellationToken cancellationToken)
    {
        LocationModel? locationModel;
        LocationSimplifiedViewModel? result = null;

        IQueryable<LocationModel> query = databaseContext.Locations;

        if (command.IncludeParkingSpaces!.Value)
        {
            locationModel = await query.Include(l => l.ParkingSpaces)
                .FirstOrDefaultAsync(l => l.Id == command.LocationId, cancellationToken);

            if (locationModel != null)
            {
                IEnumerable<ParkingSpaceSimplifiedWithoutLocationViewModel>? parkingSpace =
                    locationModel.ParkingSpaces?.Select(x =>
                        new ParkingSpaceSimplifiedWithoutLocationViewModel(x.Floor, x.ParkingSpaceName, x.IsOccupied,
                            x.ParkingSpaceType));

                result = new LocationViewModel(locationModel.Name, locationModel.Address, parkingSpace);
            }
                
        }
        else
        {
            locationModel = await query.FirstOrDefaultAsync(l => l.Id == command.LocationId, cancellationToken);

            if (locationModel != null)
                result = new LocationSimplifiedViewModel(locationModel.Name, locationModel.Address);
        }

        return result;
    }
}