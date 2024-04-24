namespace EcoPark.Domain.Aggregates.Location.ParkingSpace;

public class ParkingSpaceAggregate
{
    public Guid Id { get; private set; }
    public int Floor { get; private set; }
    public string Name { get; private set; }
    public bool IsOccupied { get; private set; }
    public EParkingSpaceType Type { get; private set; }

    private List<Reservation> _reservations = new();

    public ParkingSpaceAggregate(ParkingSpaceModel parkingSpaceModel)
    {
        Id = parkingSpaceModel.Id;
        Floor = parkingSpaceModel.Floor;
        Name = parkingSpaceModel.ParkingSpaceName;
        IsOccupied = parkingSpaceModel.IsOccupied;
        Type = parkingSpaceModel.ParkingSpaceType;
        _reservations = parkingSpaceModel.Reservations?.Select(reservation => new Reservation(reservation)).ToList() ??
                        new List<Reservation>();
    }

    public IReadOnlyCollection<Reservation> Reservations => new ReadOnlyCollection<Reservation>(_reservations);

    public void MakeReservation(Reservation reservation)
    {
        if (IsOccupied)
            throw new Exception("Parking space is already occupied");

        _reservations.Add(reservation);
    }

    public void UpdateFloor(int? floor)
    {
        if(floor != null && floor.Value != Floor)
            Floor = floor.Value;
    }

    public void UpdateParkingSpaceName(string? parkingSpaceName)
    {
        if(!string.IsNullOrWhiteSpace(parkingSpaceName) && !parkingSpaceName.Equals(Name))
            Name = parkingSpaceName;
    }

    public void UpdateParkingSpaceType(EParkingSpaceType? parkingSpaceType)
    {
        if(parkingSpaceType != null && parkingSpaceType.Value != Type)
            Type = parkingSpaceType.Value;
    }

    public void SetOccupied(bool? isOccupied)
    {
        if(isOccupied != null && isOccupied != IsOccupied)
            IsOccupied = isOccupied.Value;
    }
}