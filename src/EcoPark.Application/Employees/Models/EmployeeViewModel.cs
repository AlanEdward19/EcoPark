namespace EcoPark.Application.Employees.Models;

public class EmployeeViewModel(Guid id, string email, string firstName, string lastName, EUserType userType)
{
    public Guid Id { get; private set; } = id;
    public string Email { get; private set; } = email;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
    public string UserType { get; private set; } = userType.ToString();
}