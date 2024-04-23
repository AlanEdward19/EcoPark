namespace EcoPark.Application.Locations.Get;

public class GetLocationQueryHandler(IAggregateRepository<LocationModel> repository) : IHandler<GetLocationQuery, LocationSimplifiedViewModel?>
{
    public async Task<LocationSimplifiedViewModel?> HandleAsync(GetLocationQuery command, CancellationToken cancellationToken)
    {
        var location = await repository.GetByIdAsync(command, cancellationToken);

        if (location == null) return null;

        if (location.ParkingSpaces != null && location.ParkingSpaces.Any())
        {
            IEnumerable<ParkingSpaceSimplifiedViewModel>? parkingSpace =
                location.ParkingSpaces?.Select(x =>
                    new ParkingSpaceSimplifiedViewModel(x.Id, x.Floor, x.ParkingSpaceName, x.IsOccupied,
                        x.ParkingSpaceType));

            return new LocationViewModel(location.Id, location.Name, location.Address, location.ReservationGraceInMinutes,
                location.CancellationFeeRate, location.ReservationFeeRate, location.HourlyParkingRate, parkingSpace);
        }

        return new LocationSimplifiedViewModel(location.Id, location.Name, location.Address, location.ReservationGraceInMinutes,
            location.CancellationFeeRate, location.ReservationFeeRate, location.HourlyParkingRate);
    }
}