using Microsoft.AspNetCore.Http;

namespace EcoPark.Application.Employees.Insert;

public class InsertEmployeeCommand(): ICommand
{
    public Guid? AdministratorId { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public EUserType? UserType { get; set; }
    public MemoryStream? Image { get; private set; }
    public string? ImageFileName { get; private set; }

    public EmployeeModel ToModel(IAuthenticationService authenticationService, string? imageUrl, string? ipv4)
    {
        string hashedPassword = authenticationService.ComputeSha256Hash(this.Password!);

        CredentialsModel credentials = new(Email!.ToLower(), hashedPassword, FirstName!, LastName!, UserType!.Value, ipv4, imageUrl);

        EmployeeModel employeeModel = new(AdministratorId, credentials.Id);
        employeeModel.SetCredentials(credentials);

        return employeeModel;
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