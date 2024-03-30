namespace EcoPark.Application.Locations.List;

public class ListLocationsQueryHandler(DatabaseDbContext databaseDbContext) : IHandler<ListLocationQuery, IEnumerable<LocationSimplifiedViewModel>>
{
    public async Task<IEnumerable<LocationSimplifiedViewModel>> HandleAsync(ListLocationQuery command, CancellationToken cancellationToken)
    {
        bool hasLocationIds = command.LocationIds != null && command.LocationIds.Any();
        IQueryable<LocationModel> query = databaseDbContext.Locations;

        IEnumerable<LocationModel> locationModels;

        if (hasLocationIds)
            query = query.Where(l => command.LocationIds!.Contains(l.Id));

        if (command.IncludeParkingSpaces!.Value)
        {
            List<LocationViewModel> result = hasLocationIds
                ? new(command.LocationIds!.Count())
                : new(100);

            query = query.Include(l => l.ParkingSpaces);

            locationModels = await query.ToListAsync(cancellationToken);

            foreach (var locationModel in locationModels)
            {
                List<ParkingSpaceSimplifiedWithoutLocationViewModel>? parkingSpaces =
                    locationModel.ParkingSpaces?.Select(x =>
                        new ParkingSpaceSimplifiedWithoutLocationViewModel(x.Floor, x.ParkingSpaceName, x.IsOccupied,
                            x.ParkingSpaceType)).ToList();

                LocationViewModel location = new(locationModel.Name, locationModel.Address, parkingSpaces);

                result.Add(location);
            }

            return result;
        }
        else
        {
            List<LocationSimplifiedViewModel> result = hasLocationIds
                ? new(command.LocationIds!.Count())
                : new(100);

            locationModels = await query.ToListAsync(cancellationToken);

            foreach (var locationModel in locationModels)
            {
                LocationSimplifiedViewModel location = new(locationModel.Name, locationModel.Address);

                result.Add(location);
            }

            return result;
        }
    }
}