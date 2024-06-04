namespace EcoPark.Application.Clients.Get;

public class GetClientQuery : IQuery
{
    public Guid? ClientId { get; set; }
    public bool IncludeCars { get; set; }

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}