namespace EcoPark.Application.Reservations.Update;

public class UpdateReservationCommand : ICommand
{
    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}