namespace EcoPark.Application.Employees.Insert;

public class InsertEmployeeCommand(string? email, string? password, string? firstName, string? lastName, EUserType? userType): ICommand
{
    public string? Email { get; private set; } = email!.ToLower();
    public string? Password { get; private set; } = password;
    public string? FirstName { get; private set; } = firstName;
    public string? LastName { get; private set; } = lastName;
    public EUserType? UserType { get; private set; } = userType;

    public EmployeeModel ToModel(IAuthenticationService authenticationService)
    {
        string hashedPassword = authenticationService.ComputeSha256Hash(this.Password!);

        return new(Email!.ToLower(), hashedPassword, FirstName!, LastName!, UserType!.Value);
    }

    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}