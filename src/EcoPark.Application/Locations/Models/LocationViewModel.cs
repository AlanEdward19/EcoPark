namespace EcoPark.Application.Locations.Models;

public class LocationViewModel(Guid id, string name, string address, int reservationGraceInMinutes, double cancellationFeeRate,
        double reservationFeeRate, double hourlyParkingRate, IEnumerable<ParkingSpaceSimplifiedViewModel>? parkingSpaces) 
    : LocationSimplifiedViewModel(id, name, address, reservationGraceInMinutes, cancellationFeeRate, reservationFeeRate, hourlyParkingRate)
{
   public IEnumerable<ParkingSpaceSimplifiedViewModel>? ParkingSpaces { get; private set; } = parkingSpaces;
}