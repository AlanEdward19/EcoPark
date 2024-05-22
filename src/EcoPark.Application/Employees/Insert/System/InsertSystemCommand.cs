namespace EcoPark.Application.Employees.Insert.System;

public class InsertSystemCommand(string? email, string? password, string? firstName, string? lastName, string ipv4) : ICommand
{
    public string? Email { get; private set; } = email!.ToLower();
    public string? Password { get; private set; } = password;
    public string? FirstName { get; private set; } = firstName;
    public string? LastName { get; private set; } = lastName;
    public string Ipv4 { get; private set; } = ipv4;

    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }

    public EmployeeModel ToModel(IAuthenticationService authenticationService, Guid administratorId, string? ipv4)
    {
        string hashedPassword = authenticationService.ComputeSha256Hash(this.Password!);

        CredentialsModel credentials = new(Email!.ToLower(), hashedPassword, FirstName!, LastName!, EUserType.System, ipv4, null);

        EmployeeModel employeeModel = new(administratorId, credentials.Id);
        employeeModel.SetCredentials(credentials);

        return employeeModel;
    }
}