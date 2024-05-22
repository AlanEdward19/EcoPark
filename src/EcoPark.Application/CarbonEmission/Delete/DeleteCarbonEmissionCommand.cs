namespace EcoPark.Application.CarbonEmission.Delete;

public record DeleteCarbonEmissionCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}