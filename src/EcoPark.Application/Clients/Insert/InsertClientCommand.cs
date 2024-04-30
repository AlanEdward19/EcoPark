namespace EcoPark.Application.Clients.Insert;

public class InsertClientCommand(string email, string password, string firstName, string lastName)
    : ICommand
{
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;

    public ClientModel ToModel(IAuthenticationService authenticationService)
    {
        string hashedPassword = authenticationService.ComputeSha256Hash(Password);

        CredentialsModel credentials = new(Email.ToLower(), hashedPassword, FirstName, LastName, EUserType.Client, null);

        ClientModel clientModel = new(credentials.Id);
        clientModel.SetCredentials(credentials);

        return clientModel;
    }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}