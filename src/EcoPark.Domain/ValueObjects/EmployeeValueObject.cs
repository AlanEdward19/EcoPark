using System.Security.Cryptography;
using EcoPark.Domain.DataModels.Employee;

namespace EcoPark.Domain.ValueObjects;

public class EmployeeValueObject
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Password { get; set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public EUserType UserType { get; private set; }

    public EmployeeValueObject(Guid id, string email, string password, string firstName, string lastName,
        EUserType userType)
    {
        Id = id;
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        UserType = userType;
    }

    public EmployeeValueObject(EmployeeModel employeeModel)
    {
        Id = employeeModel.Credentials.Id;
        Email = employeeModel.Credentials.Email;
        Password = employeeModel.Credentials.Password;
        FirstName = employeeModel.Credentials.FirstName;
        LastName = employeeModel.Credentials.LastName;
        UserType = employeeModel.Credentials.UserType;
    }

    public void UpdateFirstName(string? firstName)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
            FirstName = firstName;
    }

    public void UpdateLastName(string? lastName)
    {
        if (!string.IsNullOrWhiteSpace(lastName))
            LastName = lastName;
    }

    public void UpdateEmail(string? email)
    {
        if (!string.IsNullOrWhiteSpace(email))
            Email = email.ToLower();
    }

    public void UpdatePassword(string? password)
    {
        if (!string.IsNullOrWhiteSpace(password))
            Password = password;
    }

    public void UpdateUserType(EUserType? userType)
    {
        if (userType != null && userType != UserType)
            UserType = userType.Value;
    }

    public string GetFullName() => $"{FirstName} {LastName}";
}