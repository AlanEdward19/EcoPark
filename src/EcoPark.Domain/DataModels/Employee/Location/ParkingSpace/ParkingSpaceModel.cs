﻿namespace EcoPark.Domain.DataModels.Employee.Location.ParkingSpace;

public class ParkingSpaceModel(Guid locationId, int floor, string parkingSpaceName, bool isOccupied, EParkingSpaceType parkingSpaceType)
    : BaseDataModel
{
    public Guid LocationId { get; private set; } = locationId;
    public int Floor { get; private set; } = floor;
    public string ParkingSpaceName { get; private set; } = parkingSpaceName;
    public bool IsOccupied { get; private set; } = isOccupied;
    public EParkingSpaceType ParkingSpaceType { get; private set; } = parkingSpaceType;

    [ForeignKey(nameof(LocationId))]
    public virtual LocationModel Location { get; set; }

    public virtual ICollection<ReservationModel>? Reservations { get; set; }

    public void UpdateBasedOnAggregate(ParkingSpaceAggregate parkingSpaceAggregate)
    {
        Floor = parkingSpaceAggregate.Floor;
        ParkingSpaceName = parkingSpaceAggregate.Name;
        IsOccupied = parkingSpaceAggregate.IsOccupied;
        ParkingSpaceType = parkingSpaceAggregate.Type;
        UpdatedAt = DateTime.Now;
    }
}