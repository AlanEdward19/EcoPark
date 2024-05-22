namespace EcoPark.Application.Authentication.Get;

public class LoginQuery(string email, string password) : IQuery
{
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}