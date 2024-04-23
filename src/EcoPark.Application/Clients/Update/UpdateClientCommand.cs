namespace EcoPark.Application.Clients.Update;

public class UpdateClientCommand(string? email, string? password, string? firstName, string? lastName)
    : ICommand
{
    public Guid ClientId { get; private set; }
    public string? Email { get; private set; } = email;
    public string? Password { get; private set; } = password;
    public string? FirstName { get; private set; } = firstName;
    public string? LastName { get; private set; } = lastName;

    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }

    public void SetClientId(Guid clientId)
    {
        ClientId = clientId;
    }

    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}