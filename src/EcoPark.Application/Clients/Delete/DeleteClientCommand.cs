namespace EcoPark.Application.Clients.Delete;

public record DeleteClientCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}