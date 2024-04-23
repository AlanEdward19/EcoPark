namespace EcoPark.Application.Locations.Get;

public class GetLocationQuery : IQuery
{
    public Guid LocationId { get; set; }
    public bool? IncludeParkingSpaces { get; set; } = false;

    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}