namespace EcoPark.Application.Reservations.Delete;

public record DeleteReservationCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}