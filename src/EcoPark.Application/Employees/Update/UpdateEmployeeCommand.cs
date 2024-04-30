namespace EcoPark.Application.Employees.Update;

public class UpdateEmployeeCommand(string? firstName, string? lastName, string? email, string? password, EUserType? userType) : ICommand
{
    public Guid EmployeeId { get; private set; }
    public string? FirstName { get; private set; } = firstName;
    public string? LastName { get; private set; } = lastName;
    public string? Email { get; private set; } = email?.ToLower();
    public string? Password { get; private set; } = password;
    public EUserType? UserType { get; private set; } = userType;

    public void SetEmployeeId(Guid employeeId) => EmployeeId = employeeId;

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}