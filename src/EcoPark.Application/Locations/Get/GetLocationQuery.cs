namespace EcoPark.Application.Locations.Get;

public class GetLocationQuery : IQuery
{
    public Guid LocationId { get; set; }
    public bool? IncludeParkingSpaces { get; set; } = false;
}