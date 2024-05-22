namespace EcoPark.Application.ParkingSpaces.Delete;

public record DeleteParkingSpaceCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}