namespace EcoPark.Application.Locations.List;

public class ListLocationsQueryHandler(IAggregateRepository<LocationModel> repository) : IHandler<ListLocationQuery, IEnumerable<LocationSimplifiedViewModel>>
{
    public async Task<IEnumerable<LocationSimplifiedViewModel>> HandleAsync(ListLocationQuery command, CancellationToken cancellationToken)
    {
        bool hasLocationIds = command.LocationIds != null && command.LocationIds.Any();
        var locations =  await repository.ListAsync(command, cancellationToken);

        if (command.IncludeParkingSpaces!.Value)
        {
            List<LocationViewModel> result = hasLocationIds
                ? new(command.LocationIds!.Count())
                : new(100);

            foreach (var locationModel in locations)
            {
                List<ParkingSpaceSimplifiedViewModel>? parkingSpaces =
                    locationModel.ParkingSpaces?.Select(x =>
                        new ParkingSpaceSimplifiedViewModel(x.Floor, x.ParkingSpaceName, x.IsOccupied,
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

            foreach (var locationModel in locations)
            {
                LocationSimplifiedViewModel location = new(locationModel.Name, locationModel.Address);

                result.Add(location);
            }

            return result;
        }
    }
}