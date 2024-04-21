namespace EcoPark.Application.ParkingSpaces.Update;

public class UpdateParkingSpaceCommand(int? floor, string? parkingSpaceName, EParkingSpaceType? parkingSpaceType, bool? isOccupied) : ICommand
{
    public Guid ParkingSpaceId { get; private set; }
    public int? Floor { get; private set; } = floor;
    public string? ParkingSpaceName { get; private set; } = parkingSpaceName;
    public EParkingSpaceType? ParkingSpaceType { get; private set; } = parkingSpaceType;
    public bool? IsOccupied { get; private set; } = isOccupied;

    public void SetParkingSpaceId(Guid parkingSpaceId) => ParkingSpaceId = parkingSpaceId;
}