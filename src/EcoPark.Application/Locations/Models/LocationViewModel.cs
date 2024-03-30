namespace EcoPark.Application.Locations.Models;

public class LocationViewModel(string name, string address, IEnumerable<ParkingSpaceSimplifiedWithoutLocationViewModel>? parkingSpaces) 
    : LocationSimplifiedViewModel(name, address)
{
   public IEnumerable<ParkingSpaceSimplifiedWithoutLocationViewModel>? ParkingSpaces { get; private set; } = parkingSpaces;
}