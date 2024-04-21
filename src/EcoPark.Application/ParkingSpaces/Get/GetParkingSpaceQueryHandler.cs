namespace EcoPark.Application.ParkingSpaces.Get;

public class GetParkingSpaceQueryHandler(IAggregateRepository<ParkingSpaceModel> repository) : IHandler<GetParkingSpaceQuery, ParkingSpaceSimplifiedViewModel?>
{
    public async Task<ParkingSpaceSimplifiedViewModel?> HandleAsync(GetParkingSpaceQuery command,
        CancellationToken cancellationToken)
    {
        ParkingSpaceSimplifiedViewModel? result = null;
        var parkingSpace = await repository.GetByIdAsync(command, cancellationToken);

        if (parkingSpace != null)
        {
            if (command.IncludeReservations)
            {
                IEnumerable<ReservationSimplifiedViewModel>? reservations = parkingSpace.Reservations?.Select(x =>
                    new ReservationSimplifiedViewModel(x.CardId, x.ClientId, x.ReservationCode, x.Status,
                        x.ReservationDate, x.ExpirationDate));

                result = new ParkingSpaceViewModel(parkingSpace.Floor, parkingSpace.ParkingSpaceName,
                    parkingSpace.IsOccupied, parkingSpace.ParkingSpaceType, reservations);
            }

            else
                result = new ParkingSpaceSimplifiedViewModel(parkingSpace.Floor,
                    parkingSpace.ParkingSpaceName, parkingSpace.IsOccupied,
                    parkingSpace.ParkingSpaceType);
        }

        return result;
    }
}