namespace EcoPark.Application.ParkingSpaces.Models;

public class ParkingSpaceSimplifiedViewModel(int floor, string name, bool isOccupied, EParkingSpaceType type, LocationSimplifiedViewModel location) 
    : ParkingSpaceSimplifiedWithoutLocationViewModel(floor, name, isOccupied, type)
{
    public LocationSimplifiedViewModel Location { get; private set; } = location;
}