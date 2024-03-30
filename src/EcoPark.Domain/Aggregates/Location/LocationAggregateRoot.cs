using EcoPark.Domain.DataModels;

namespace EcoPark.Domain.Aggregates.Location;

public class LocationAggregateRoot
{
    private List<ParkingSpaceAggregate> _parkingSpaces = new();

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public IReadOnlyCollection<ParkingSpaceAggregate> ParkingSpaces => new ReadOnlyCollection<ParkingSpaceAggregate>(_parkingSpaces);

    public LocationAggregateRoot(LocationModel locationModel)
    {
        Id = locationModel.Id;
        Name = locationModel.Name;
        Address = locationModel.Address;

        _parkingSpaces = locationModel.ParkingSpaces?
            .Select(parkingSpaceModel => new ParkingSpaceAggregate(parkingSpaceModel)).ToList() ?? new();
    }

    public void UpdateName(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;
    }

    public void UpdateAddress(string? address)
    {
        if (!string.IsNullOrWhiteSpace(address))
            Address = address;
    }

    public void AddParkingSpace(ParkingSpaceAggregate parkingSpace) => _parkingSpaces.Add(parkingSpace);
    public void RemoveParkingSpace(ParkingSpaceAggregate parkingSpace) => _parkingSpaces.Remove(parkingSpace);
}