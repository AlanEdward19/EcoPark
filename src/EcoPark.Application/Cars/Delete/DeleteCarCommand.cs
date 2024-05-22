namespace EcoPark.Application.Cars.Delete;

public record DeleteCarCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}