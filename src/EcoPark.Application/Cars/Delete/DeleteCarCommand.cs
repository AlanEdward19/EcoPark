namespace EcoPark.Application.Cars.Delete;

public record DeleteCarCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}