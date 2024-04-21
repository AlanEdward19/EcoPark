namespace EcoPark.Application.ParkingSpaces.Models;

public class ParkingSpaceViewModel(int floor, string name, bool isOccupied, EParkingSpaceType type, 
        IEnumerable<ReservationSimplifiedViewModel>? reservations) : ParkingSpaceSimplifiedViewModel(floor, name, isOccupied, type)
{
    public IEnumerable<ReservationSimplifiedViewModel>? Reservations { get; private set; } = reservations;
}