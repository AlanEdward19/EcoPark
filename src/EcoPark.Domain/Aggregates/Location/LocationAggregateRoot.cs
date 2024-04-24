namespace EcoPark.Domain.Aggregates.Location;

public class LocationAggregateRoot
{
    private List<ParkingSpaceAggregate> _parkingSpaces = new();

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public int ReservationGraceInMinutes { get; private set; }
    public double CancellationFeeRate { get; private set; }
    public double ReservationFeeRate { get; private set; }
    public double HourlyParkingRate { get; private set; }
    public IReadOnlyCollection<ParkingSpaceAggregate> ParkingSpaces => new ReadOnlyCollection<ParkingSpaceAggregate>(_parkingSpaces);

    public LocationAggregateRoot(Guid id, string name, string address, int reservationGraceInMinutes,
        double cancellationFeeRate, double reservationFeeRate, double hourlyParkingRate)
    {
        Id = id;
        Name = name;
        Address = address;
        ReservationGraceInMinutes = reservationGraceInMinutes;
        CancellationFeeRate = cancellationFeeRate;
        ReservationFeeRate = reservationFeeRate;
        HourlyParkingRate = hourlyParkingRate;
    }

    public LocationAggregateRoot(LocationModel locationModel)
    {
        Id = locationModel.Id;
        Name = locationModel.Name;
        Address = locationModel.Address;
        ReservationGraceInMinutes = locationModel.ReservationGraceInMinutes;
        CancellationFeeRate = locationModel.CancellationFeeRate;
        ReservationFeeRate = locationModel.ReservationFeeRate;
        HourlyParkingRate = locationModel.HourlyParkingRate;

        _parkingSpaces = locationModel.ParkingSpaces?
            .Select(parkingSpaceModel => new ParkingSpaceAggregate(parkingSpaceModel)).ToList() ?? new();
    }

    public void UpdateName(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name) && !name.Equals(Name))
            Name = name;
    }

    public void UpdateAddress(string? address)
    {
        if (!string.IsNullOrWhiteSpace(address) && !address.Equals(Address))
            Address = address;
    }

    public void UpdateReservationGraceInMinutes(int? reservationGraceInMinutes)
    {
        if (reservationGraceInMinutes is >= 0 && reservationGraceInMinutes != ReservationGraceInMinutes)
            ReservationGraceInMinutes = reservationGraceInMinutes.Value;
    }

    public void UpdateCancellationFeeRate(double? cancellationFeeRate)
    {
        if (cancellationFeeRate is >= 0 && cancellationFeeRate != CancellationFeeRate)
            CancellationFeeRate = cancellationFeeRate.Value;
    }

    public void UpdateReservationFeeRate(double? reservationFeeRate)
    {
        if (reservationFeeRate is >= 0 && reservationFeeRate != ReservationFeeRate)
            ReservationFeeRate = reservationFeeRate.Value;
    }

    public void UpdateHourlyParkingRate(double? hourlyParkingRate)
    {
        if (hourlyParkingRate is >= 0 && hourlyParkingRate != HourlyParkingRate)
            HourlyParkingRate = hourlyParkingRate.Value;
    }

    public void AddParkingSpace(ParkingSpaceAggregate parkingSpace) => _parkingSpaces.Add(parkingSpace);
    public void RemoveParkingSpace(ParkingSpaceAggregate parkingSpace) => _parkingSpaces.Remove(parkingSpace);
}