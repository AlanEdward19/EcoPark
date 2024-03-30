namespace EcoPark.Application.ParkingSpaces.Get;

public class GetParkingSpaceQuery
{
    public Guid ParkingSpaceId { get; set; }
    public bool IncludeReservations { get; set; }
}