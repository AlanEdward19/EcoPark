namespace EcoPark.Application.Locations.Get;

public class GetLocationQuery
{
    public Guid LocationId { get; set; }
    public bool? IncludeParkingSpaces { get; set; } = false;
}