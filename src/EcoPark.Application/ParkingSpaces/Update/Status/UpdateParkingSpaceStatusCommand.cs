namespace EcoPark.Application.ParkingSpaces.Update.Status;

public class UpdateParkingSpaceStatusCommand : ICommand
{
    public Guid Id { get; private set; }
    public bool Status { get; private set; }

    public void SetParkingSpaceId(Guid id)
    {
        Id = id;
    }

    public void SetStatus(bool status)
    {
        Status = status;
    }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}