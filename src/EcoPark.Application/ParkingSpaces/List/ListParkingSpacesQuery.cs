namespace EcoPark.Application.ParkingSpaces.List;

public class ListParkingSpacesQuery
{
    public IEnumerable<Guid>? ParkingSpaceIds { get; set; }
    public bool? IncludeReservations { get; set; } = false;
}