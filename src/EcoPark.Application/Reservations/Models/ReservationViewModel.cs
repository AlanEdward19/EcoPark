﻿namespace EcoPark.Application.Reservations.Models;

public class ReservationViewModel(Guid id, Guid? carId, Guid? clientId, string reservationCode, double punctuation, EReservationStatus status, DateTime reservationDate, 
    DateTime expirationDate, ParkingSpaceSimplifiedViewModel parkingSpace) 
    : ReservationSimplifiedViewModel(id, carId, clientId, reservationCode, punctuation, status, reservationDate, expirationDate)
{
    public ParkingSpaceSimplifiedViewModel ParkingSpace { get; private set; } = parkingSpace;
}