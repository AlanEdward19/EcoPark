namespace EcoPark.Application.Locations.Update;

public class UpdateLocationCommand(string? name, string? address, int? reservationGraceInMinutes, 
    double? cancellationFeeRate, double? reservationFeeRate, double? hourlyParkingRate) : ICommand
{
    public Guid LocationId { get; private set; }
    public string? Name { get; private set; } = name;
    public string? Address { get; private set; } = address;
    public int? ReservationGraceInMinutes { get; private set; } = reservationGraceInMinutes;
    public double? CancellationFeeRate { get; private set; } = cancellationFeeRate;
    public double? ReservationFeeRate { get; private set; } = reservationFeeRate;
    public double? HourlyParkingRate { get; private set; } = hourlyParkingRate;

    public void SetLocationId(Guid locationId) => LocationId = locationId;

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}