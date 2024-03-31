namespace EcoPark.Application.Employees.Update;

public class UpdateEmployeeCommand(string? firstName, string? lastName, string? email, string? password, EUserType? userType)
{
    public Guid EmployeeId { get; private set; }
    public string? FirstName { get; private set; } = firstName;
    public string? LastName { get; private set; } = lastName;
    public string? Email { get; private set; } = email;
    public string? Password { get; private set; } = password;
    public EUserType? UserType { get; private set; } = userType;

    public void SetEmployeeId(Guid employeeId) => EmployeeId = employeeId;
}