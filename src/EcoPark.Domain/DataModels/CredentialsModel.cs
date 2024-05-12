namespace EcoPark.Domain.DataModels;

public class CredentialsModel(string email, string password, string firstName, string lastName, EUserType userType, string? ipv4, string? image)
    : BaseDataModel
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public EUserType UserType { get; set; } = userType;
    public string? Ipv4 { get; set; } = ipv4;
    public string? Image { get; set; } = image;
}