using EcoPark.Domain.Aggregates.Location;

namespace EcoPark.Domain.DataModels.Employee.Location;

public class LocationModel(Guid ownerId, string name, string address, int reservationGraceInMinutes, double cancellationFeeRate,
    double reservationFeeRate, double hourlyParkingRate) : BaseDataModel
{
    public Guid OwnerId { get; set; } = ownerId;
    public string Name { get; private set; } = name;
    public string Address { get; private set; } = address;
    public int ReservationGraceInMinutes { get; private set; } = reservationGraceInMinutes;
    public double CancellationFeeRate { get; private set; } = cancellationFeeRate;
    public double ReservationFeeRate { get; private set; } = reservationFeeRate;
    public double HourlyParkingRate { get; private set; } = hourlyParkingRate;

    [ForeignKey(nameof(OwnerId))]
    public virtual EmployeeModel Owner { get; set; }
    public virtual ICollection<ParkingSpaceModel>? ParkingSpaces { get; set; }

    public virtual GroupAccessModel? GroupAccess { get; set; }

    public void UpdateBasedOnAggregate(LocationAggregateRoot locationAggregate)
    {
        Name = locationAggregate.Name;
        Address = locationAggregate.Address;
        ReservationGraceInMinutes = locationAggregate.ReservationGraceInMinutes;
        CancellationFeeRate = locationAggregate.CancellationFeeRate;
        ReservationFeeRate = locationAggregate.ReservationFeeRate;
        HourlyParkingRate = locationAggregate.HourlyParkingRate;
        UpdatedAt = DateTime.Now;
    }
}