namespace EcoPark.Application.Locations.List;

public class ListLocationQuery(IEnumerable<Guid>? locationIds, bool? includeParkingSpaces) : IQuery
{
    public IEnumerable<Guid>? LocationIds { get; private set; } = locationIds;
    public bool? IncludeParkingSpaces { get; private set; } = includeParkingSpaces ?? false;

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}