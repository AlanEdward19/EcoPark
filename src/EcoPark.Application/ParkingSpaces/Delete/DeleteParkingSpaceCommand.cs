namespace EcoPark.Application.ParkingSpaces.Delete;

public record DeleteParkingSpaceCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}