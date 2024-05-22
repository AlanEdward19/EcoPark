using Microsoft.AspNetCore.Http;

namespace EcoPark.Application.Clients.Insert;

public class InsertClientCommand : ICommand
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
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
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
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