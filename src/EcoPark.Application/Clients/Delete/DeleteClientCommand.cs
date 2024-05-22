namespace EcoPark.Application.Clients.Delete;

public record DeleteClientCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}