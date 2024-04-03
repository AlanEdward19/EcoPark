namespace EcoPark.Application.ParkingSpaces.Insert;

public class InsertParkingSpaceCommand(Guid? locationId, int? floor, string? parkingSpaceName, bool? isOccupied, EParkingSpaceType? type)
{
    public Guid? LocationId { get; private set; } = locationId;
    public int? Floor { get; private set; } = floor;
    public string? ParkingSpaceName { get; private set; } = parkingSpaceName;
    public bool? IsOccupied { get; private set; } = isOccupied;
    public EParkingSpaceType? Type { get; private set; } = type;
}