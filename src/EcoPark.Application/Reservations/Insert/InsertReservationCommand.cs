namespace EcoPark.Application.Reservations.Insert;

public class InsertReservationCommand : ICommand
{
    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}