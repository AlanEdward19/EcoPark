namespace EcoPark.Application.Clients.Get;

public class GetClientQuery : IQuery
{
    public Guid ClientId { get; set; }
    public bool IncludeCars { get; set; }

    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}