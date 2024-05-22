namespace EcoPark.Application.ParkingSpaces.Get;

public class GetParkingSpaceQuery : IQuery
{
    public Guid ParkingSpaceId { get; set; }
    public bool IncludeReservations { get; set; }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}