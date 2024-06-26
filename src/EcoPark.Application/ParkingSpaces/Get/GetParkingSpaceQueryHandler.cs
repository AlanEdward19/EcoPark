﻿namespace EcoPark.Application.ParkingSpaces.Get;

public class GetParkingSpaceQueryHandler(IRepository<ParkingSpaceModel> repository) : IHandler<GetParkingSpaceQuery, ParkingSpaceSimplifiedViewModel?>
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
                    new ReservationSimplifiedViewModel(x.Id, x.CarId, x.ClientId, x.ReservationCode, x.Punctuation, x.Status,
                        x.ReservationDate, x.ExpirationDate));

                result = new ParkingSpaceViewModel(parkingSpace.Id, parkingSpace.Floor, parkingSpace.ParkingSpaceName,
                    parkingSpace.IsOccupied, parkingSpace.ParkingSpaceType, reservations);
            }

            else
                result = new ParkingSpaceSimplifiedViewModel(parkingSpace.Id, parkingSpace.Floor,
                    parkingSpace.ParkingSpaceName, parkingSpace.IsOccupied,
                    parkingSpace.ParkingSpaceType);
        }

        return result;
    }
}