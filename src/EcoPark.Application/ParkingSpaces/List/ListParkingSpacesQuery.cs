namespace EcoPark.Application.ParkingSpaces.List;

public class ListParkingSpacesQuery : IQuery
{
    public IEnumerable<Guid>? ParkingSpaceIds { get; set; }
    public bool? IncludeReservations { get; set; } = false;

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}