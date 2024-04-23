namespace EcoPark.Application.Locations.Models;

public class LocationSimplifiedViewModel(string name, string address, int reservationGraceInMinutes, double cancellationFeeRate,
    double reservationFeeRate, double hourlyParkingRate)
{
    public string Name { get; private set; } = name;
    public string Address { get; private set; } = address;
    public int ReservationGraceInMinutes { get; private set; } = reservationGraceInMinutes;
    public double CancellationFeeRate { get; private set; } = cancellationFeeRate;
    public double ReservationFeeRate { get; private set; } = reservationFeeRate;
    public double HourlyParkingRate { get; private set; } = hourlyParkingRate;
}