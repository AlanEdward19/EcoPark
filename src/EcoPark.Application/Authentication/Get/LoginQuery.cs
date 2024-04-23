namespace EcoPark.Application.Authentication.Get;

public class LoginQuery(string email, string password) : IQuery
{
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
    public bool IsEmployee { get; private set; }

    public void SetIsEmployee(bool isEmployee) => IsEmployee = isEmployee;

    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}