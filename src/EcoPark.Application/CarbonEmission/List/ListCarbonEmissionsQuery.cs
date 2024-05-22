namespace EcoPark.Application.CarbonEmission.List;

public class ListCarbonEmissionsQuery(IEnumerable<Guid> carbonEmissionsIds) : IQuery
{
    public IEnumerable<Guid> CarbonEmissionsIds { get; private set; } = carbonEmissionsIds;

    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}