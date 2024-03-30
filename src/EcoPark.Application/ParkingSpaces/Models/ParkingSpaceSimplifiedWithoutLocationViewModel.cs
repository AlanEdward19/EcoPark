namespace EcoPark.Application.ParkingSpaces.Models;

public class ParkingSpaceSimplifiedWithoutLocationViewModel(int floor, string name, bool isOccupied, EParkingSpaceType type)
{
    public int Floor { get; private set; } = floor;
    public string Name { get; private set; } = name;
    public bool IsOccupied { get; private set; } = isOccupied;
    public string Type { get; private set; } = type.ToString();
}