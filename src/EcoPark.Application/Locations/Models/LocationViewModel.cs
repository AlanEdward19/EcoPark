namespace EcoPark.Application.Locations.Models;

public class LocationViewModel(string name, string address, IEnumerable<ParkingSpaceSimplifiedViewModel>? parkingSpaces) 
    : LocationSimplifiedViewModel(name, address)
{
   public IEnumerable<ParkingSpaceSimplifiedViewModel>? ParkingSpaces { get; private set; } = parkingSpaces;
}