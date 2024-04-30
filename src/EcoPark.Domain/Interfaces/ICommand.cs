namespace EcoPark.Domain.Interfaces;

public interface ICommand
{
    public (string Email, EUserType UserType) RequestUserInfo { get;}

    public void SetRequestUserInfo((string email, EUserType userType) information);
}