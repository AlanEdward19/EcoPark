namespace EcoPark.Domain.DataModels;

public class EmployeeModel(string email, string password, string firstName, string lastName, EUserType userType) 
    : UserModel(email, password, firstName, lastName)
{
    public EUserType UserType { get; private set; }
}