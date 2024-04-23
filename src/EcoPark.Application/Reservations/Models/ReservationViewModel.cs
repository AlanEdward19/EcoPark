namespace EcoPark.Application.Reservations.Models;

public class ReservationViewModel(Guid carId, Guid clientId, string reservationCode, EReservationStatus status, DateTime reservationDate, DateTime expirationDate, 
        ParkingSpaceSimplifiedViewModel parkingSpace) : ReservationSimplifiedViewModel(carId, clientId, reservationCode, status, reservationDate, expirationDate)
{
    public ParkingSpaceSimplifiedViewModel ParkingSpace { get; private set; } = parkingSpace;
}