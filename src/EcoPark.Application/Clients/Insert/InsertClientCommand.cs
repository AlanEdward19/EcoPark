using Microsoft.AspNetCore.Http;

namespace EcoPark.Application.Clients.Insert;

public class InsertClientCommand(string email, string password, string firstName, string lastName)
    : ICommand
{
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
    public MemoryStream? Image { get; private set; }
    public string? ImageFileName { get; private set; }

    public ClientModel ToModel(IAuthenticationService authenticationService, string? imageUrl)
    {
        string hashedPassword = authenticationService.ComputeSha256Hash(Password);

        CredentialsModel credentials = new(Email.ToLower(), hashedPassword, FirstName, LastName, EUserType.Client, null, imageUrl);

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

    public async Task SetImage(IFormFile? image, string imageFileName, CancellationToken cancellationToken)
    {
        Image = new();

        await image.CopyToAsync(Image, cancellationToken);
        ImageFileName = imageFileName;
    }
}