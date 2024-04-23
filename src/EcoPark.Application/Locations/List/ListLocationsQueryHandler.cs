﻿namespace EcoPark.Application.Locations.List;

public class ListLocationsQueryHandler(IAggregateRepository<LocationModel> repository) : IHandler<ListLocationQuery, IEnumerable<LocationSimplifiedViewModel>>
{
    public async Task<IEnumerable<LocationSimplifiedViewModel>> HandleAsync(ListLocationQuery command, CancellationToken cancellationToken)
    {
        var locations =  await repository.ListAsync(command, cancellationToken);

        if (locations == null || !locations.Any())
            return Enumerable.Empty<LocationSimplifiedViewModel>();

        if (command.IncludeParkingSpaces!.Value)
        {
            List<LocationViewModel> result = new(command.LocationIds!.Count());

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
            List<LocationSimplifiedViewModel> result = new(command.LocationIds!.Count());

            foreach (var locationModel in locations)
            {
                LocationSimplifiedViewModel location = new(locationModel.Name, locationModel.Address);

                result.Add(location);
            }

            return result;
        }
    }
}