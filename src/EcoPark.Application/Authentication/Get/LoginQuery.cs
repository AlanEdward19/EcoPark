namespace EcoPark.Application.Authentication.Get;

public class LoginQuery(string email, string password)
{
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
    public bool IsEmployee { get; private set; }

    public void SetIsEmployee(bool isEmployee) => IsEmployee = isEmployee;
}