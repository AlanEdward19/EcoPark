using EcoPark.Domain.ValueObjects;

namespace EcoPark.Domain.DataModels;

public class EmployeeModel(string email, string password, string firstName, string lastName, EUserType userType) 
    : UserModel(email, password, firstName, lastName)
{
    public EUserType UserType { get; private set; } = userType;

    public void UpdateBasedOnValueObject(EmployeeValueObject employeeValueObject)
    {
        Email = employeeValueObject.Email;
        Password = employeeValueObject.Password;
        FirstName = employeeValueObject.FirstName;
        LastName = employeeValueObject.LastName;
        UserType = employeeValueObject.UserType;
    }
}