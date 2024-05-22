namespace EcoPark.Application.ParkingSpaces.List;

public class ListParkingSpacesQuery : IQuery
{
    public IEnumerable<Guid>? ParkingSpaceIds { get; set; }
    public bool? IncludeReservations { get; set; } = false;

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}