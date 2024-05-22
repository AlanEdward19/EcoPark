namespace EcoPark.Application.ParkingSpaces.Models;

public class ParkingSpaceSimplifiedViewModel(Guid id, int floor, string name, bool isOccupied, EParkingSpaceType type)
{
    public Guid Id { get; private set; } = id;
    public int Floor { get; private set; } = floor;
    public string Name { get; private set; } = name;
    public bool IsOccupied { get; private set; } = isOccupied;
    public string Type { get; private set; } = type.ToString();
}