namespace EcoPark.Application.Reservations.Delete;

public record DeleteReservationCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}