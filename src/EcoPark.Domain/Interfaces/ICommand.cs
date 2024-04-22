namespace EcoPark.Domain.Interfaces;

public interface ICommand
{
    public (string Email, string UserType) RequestUserInfo { get;}

    public void SetRequestUserInfo((string email, string userType) information);
}