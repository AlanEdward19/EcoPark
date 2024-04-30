namespace EcoPark.Application.Clients.List;

public class ListClientsQuery(IEnumerable<Guid>? clientIds, bool includeCars) : IQuery
{
    public IEnumerable<Guid>? ClientIds { get; private set; } = clientIds;
    public bool IncludeCars { get; private set; } = includeCars;

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}