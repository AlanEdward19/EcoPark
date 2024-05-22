namespace EcoPark.Domain.ValueObjects;

public class RequestUserInfoValueObject(string email, EUserType userType)
{
    public string Email { get; private set; } = email;
    public EUserType UserType { get; private set; } = userType;
}