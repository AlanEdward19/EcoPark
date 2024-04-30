namespace EcoPark.Domain.Interfaces;

public interface IQuery
{
    public (string Email, EUserType UserType) RequestUserInfo { get; }

    public void SetRequestUserInfo((string email, EUserType userType) information);
}