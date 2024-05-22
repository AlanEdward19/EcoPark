namespace EcoPark.Application.ParkingSpaces.Get;

public class GetParkingSpaceQuery : IQuery
{
    public Guid ParkingSpaceId { get; set; }
    public bool IncludeReservations { get; set; }

    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}