using Microsoft.AspNetCore.Http;

namespace EcoPark.Application.Clients.Update;

public class UpdateClientCommand(string? email, string? password, string? firstName, string? lastName)
    : ICommand
{
    public Guid ClientId { get; private set; }
    public string? Email { get; private set; } = email;
    public string? Password { get; private set; } = password;
    public string? FirstName { get; private set; } = firstName;
    public string? LastName { get; private set; } = lastName;
    public MemoryStream? Image { get; private set; }
    public string? ImageFileName { get; private set; }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetClientId(Guid clientId)
    {
        ClientId = clientId;
    }

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