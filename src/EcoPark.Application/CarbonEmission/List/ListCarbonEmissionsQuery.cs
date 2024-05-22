namespace EcoPark.Application.CarbonEmission.List;

public class ListCarbonEmissionsQuery(IEnumerable<Guid> carbonEmissionsIds) : IQuery
{
    public IEnumerable<Guid> CarbonEmissionsIds { get; private set; } = carbonEmissionsIds;

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}