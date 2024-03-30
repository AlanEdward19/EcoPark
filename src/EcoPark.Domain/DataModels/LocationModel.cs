using EcoPark.Domain.Aggregates.Location;

namespace EcoPark.Domain.DataModels;

public class LocationModel(string name, string address) : BaseDataModel
{
    public string Name { get; set; } = name;
    public string Address { get; set; } = address;

    public virtual ICollection<ParkingSpaceModel>? ParkingSpaces { get; set; }

    public void UpdateBasedOnAggregate(LocationAggregateRoot locationAggregate)
    {
        Name = locationAggregate.Name;
        Address = locationAggregate.Address;
        UpdatedAt = DateTime.Now;

        if(locationAggregate.ParkingSpaces.Any())
            ParkingSpaces = locationAggregate.ParkingSpaces.Select(parkingSpace =>
                           new ParkingSpaceModel(locationAggregate.Id, parkingSpace.Floor, parkingSpace.Name,
                                              parkingSpace.IsOccupied, parkingSpace.Type)).ToList();
    }
}