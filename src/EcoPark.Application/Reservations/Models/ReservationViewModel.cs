namespace EcoPark.Application.Reservations.Models;

public class ReservationViewModel(Guid cardId, Guid clientId, string reservationCode, EReservationStatus status, DateTime reservationDate, DateTime expirationDate, 
        ParkingSpaceSimplifiedViewModel parkingSpace) : ReservationSimplifiedViewModel(cardId, clientId, reservationCode, status, reservationDate, expirationDate)
{
    public ParkingSpaceSimplifiedViewModel ParkingSpace { get; private set; } = parkingSpace;
}