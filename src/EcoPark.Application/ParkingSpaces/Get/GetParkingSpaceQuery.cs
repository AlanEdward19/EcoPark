namespace EcoPark.Application.ParkingSpaces.Get;

public class GetParkingSpaceQuery : IQuery
{
    public Guid ParkingSpaceId { get; set; }
    public bool IncludeReservations { get; set; }

    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}