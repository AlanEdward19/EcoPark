namespace EcoPark.Application.ParkingSpaces.List;

public class ListParkingSpacesQueryHandler(IAggregateRepository<ParkingSpaceModel> repository)
    : IHandler<ListParkingSpacesQuery, IEnumerable<ParkingSpaceSimplifiedViewModel>?>
{
    public async Task<IEnumerable<ParkingSpaceSimplifiedViewModel>?> HandleAsync(ListParkingSpacesQuery command, CancellationToken cancellationToken)
    {
        var parkingSpaces = await repository.ListAsync(command, cancellationToken);

        if (command.IncludeReservations!.Value)
        {
            List<ParkingSpaceViewModel>? result = new(parkingSpaces.Count());

            foreach (var parkingSpaceModel in parkingSpaces)
            {
                IEnumerable<ReservationSimplifiedViewModel>? reservations = parkingSpaceModel.Reservations?.Select(x =>
                    new ReservationSimplifiedViewModel(x.Id, x.CarId, x.ClientId, x.ReservationCode, x.Punctuation, x.Status,
                        x.ReservationDate, x.ExpirationDate));

                ParkingSpaceViewModel parkingSpace = new(parkingSpaceModel.Id, parkingSpaceModel.Floor,
                    parkingSpaceModel.ParkingSpaceName,
                    parkingSpaceModel.IsOccupied, parkingSpaceModel.ParkingSpaceType, reservations);

                result.Add(parkingSpace);
            }

            return result;
        }
        else
        {
            List<ParkingSpaceSimplifiedViewModel>? result = new(parkingSpaces.Count());

            foreach (var parkingSpaceModel in parkingSpaces)
            {
                ParkingSpaceSimplifiedViewModel parkingSpace = new(parkingSpaceModel.Id, parkingSpaceModel.Floor,
                    parkingSpaceModel.ParkingSpaceName,
                    parkingSpaceModel.IsOccupied, parkingSpaceModel.ParkingSpaceType);

                result.Add(parkingSpace);
            }

            return result;
        }

    }
}