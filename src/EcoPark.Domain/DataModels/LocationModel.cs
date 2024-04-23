﻿using EcoPark.Domain.Aggregates.Location;

namespace EcoPark.Domain.DataModels;

public class LocationModel(string name, string address, int reservationGraceInMinutes, double cancellationFeeRate,
    double reservationFeeRate, double hourlyParkingRate) : BaseDataModel
{
    public string Name { get; private set; } = name;
    public string Address { get; private set; } = address;
    public int ReservationGraceInMinutes { get; private set; } = reservationGraceInMinutes;
    public double CancellationFeeRate { get; private set; } = cancellationFeeRate;
    public double ReservationFeeRate { get; private set; } = reservationFeeRate;
    public double HourlyParkingRate { get; private set; } = hourlyParkingRate;

    public virtual ICollection<ParkingSpaceModel>? ParkingSpaces { get; set; }

    public void UpdateBasedOnAggregate(LocationAggregateRoot locationAggregate)
    {
        Name = locationAggregate.Name;
        Address = locationAggregate.Address;
        ReservationGraceInMinutes = locationAggregate.ReservationGraceInMinutes;
        CancellationFeeRate = locationAggregate.CancellationFeeRate;
        ReservationFeeRate = locationAggregate.ReservationFeeRate;
        HourlyParkingRate = locationAggregate.HourlyParkingRate;
        UpdatedAt = DateTime.Now;

        if(locationAggregate.ParkingSpaces.Any())
            ParkingSpaces = locationAggregate.ParkingSpaces.Select(parkingSpace =>
                           new ParkingSpaceModel(locationAggregate.Id, parkingSpace.Floor, parkingSpace.Name,
                                              parkingSpace.IsOccupied, parkingSpace.Type)).ToList();
    }
}