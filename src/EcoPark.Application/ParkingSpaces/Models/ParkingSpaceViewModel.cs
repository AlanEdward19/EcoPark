namespace EcoPark.Application.ParkingSpaces.Models;

public class ParkingSpaceViewModel(int floor, string name, bool isOccupied, EParkingSpaceType type, LocationSimplifiedViewModel location, 
        IEnumerable<ReservationSimplifiedViewModel>? reservations) : ParkingSpaceSimplifiedViewModel(floor, name, isOccupied, type, location)
{
    public IEnumerable<ReservationSimplifiedViewModel>? Reservations { get; private set; } = reservations;
}