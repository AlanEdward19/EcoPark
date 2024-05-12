namespace EcoPark.Application.Locations.List;

public class ListLocationsQueryHandler(IRepository<LocationModel> repository) : IHandler<ListLocationQuery, IEnumerable<LocationSimplifiedViewModel>>
{
    public async Task<IEnumerable<LocationSimplifiedViewModel>> HandleAsync(ListLocationQuery command, CancellationToken cancellationToken)
    {
        var locations = await repository.ListAsync(command, cancellationToken);

        if (locations == null || !locations.Any())
            return Enumerable.Empty<LocationSimplifiedViewModel>();

        if (command.IncludeParkingSpaces!.Value)
        {
            List<LocationViewModel> result = new(command.LocationIds!.Count());

            foreach (var locationModel in locations)
            {
                List<ParkingSpaceSimplifiedViewModel>? parkingSpaces =
                    locationModel.ParkingSpaces?.Select(x =>
                        new ParkingSpaceSimplifiedViewModel(x.Id, x.Floor, x.ParkingSpaceName, x.IsOccupied,
                            x.ParkingSpaceType)).ToList();

                LocationViewModel location = new(locationModel.Id, locationModel.Name, locationModel.Address, locationModel.ReservationGraceInMinutes,
                    locationModel.CancellationFeeRate, locationModel.ReservationFeeRate, locationModel.HourlyParkingRate, parkingSpaces);

                result.Add(location);
            }

            return result;
        }
        else
        {
            List<LocationSimplifiedViewModel> result = new(command.LocationIds!.Count());

            foreach (var locationModel in locations)
            {
                LocationSimplifiedViewModel location = new(locationModel.Id, locationModel.Name, locationModel.Address, locationModel.ReservationGraceInMinutes,
                    locationModel.CancellationFeeRate, locationModel.ReservationFeeRate, locationModel.HourlyParkingRate);

                result.Add(location);
            }

            return result;
        }
    }
}