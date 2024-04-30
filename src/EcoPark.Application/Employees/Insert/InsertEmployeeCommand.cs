namespace EcoPark.Application.Employees.Insert;

public class InsertEmployeeCommand(Guid? administratorId, string? email, string? password, string? firstName, string? lastName, EUserType? userType): ICommand
{
    public Guid? AdministratorId { get; private set; } = administratorId;
    public string? Email { get; private set; } = email!.ToLower();
    public string? Password { get; private set; } = password;
    public string? FirstName { get; private set; } = firstName;
    public string? LastName { get; private set; } = lastName;
    public EUserType? UserType { get; private set; } = userType;

    public EmployeeModel ToModel(IAuthenticationService authenticationService)
    {
        string hashedPassword = authenticationService.ComputeSha256Hash(this.Password!);

        CredentialsModel credentials = new(Email!.ToLower(), hashedPassword, FirstName!, LastName!, UserType!.Value, null);

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
}