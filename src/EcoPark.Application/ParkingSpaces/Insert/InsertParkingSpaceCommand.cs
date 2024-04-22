﻿namespace EcoPark.Application.ParkingSpaces.Insert;

public class InsertParkingSpaceCommand(Guid? locationId, int? floor, string? parkingSpaceName, bool? isOccupied, EParkingSpaceType? type) : ICommand
{
    public Guid? LocationId { get; private set; } = locationId;
    public int? Floor { get; private set; } = floor;
    public string? ParkingSpaceName { get; private set; } = parkingSpaceName;
    public bool? IsOccupied { get; private set; } = isOccupied;
    public EParkingSpaceType? Type { get; private set; } = type;

    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}