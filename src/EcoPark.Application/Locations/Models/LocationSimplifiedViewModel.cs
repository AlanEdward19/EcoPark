namespace EcoPark.Application.Locations.Models;

public class LocationSimplifiedViewModel(Guid id, string name, string address, int reservationGraceInMinutes, double cancellationFeeRate,
    double reservationFeeRate, double hourlyParkingRate)
{
    public Guid Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public string Address { get; private set; } = address;
    public int ReservationGraceInMinutes { get; private set; } = reservationGraceInMinutes;
    public double CancellationFeeRate { get; private set; } = cancellationFeeRate;
    public double ReservationFeeRate { get; private set; } = reservationFeeRate;
    public double HourlyParkingRate { get; private set; } = hourlyParkingRate;
}