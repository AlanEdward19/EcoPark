namespace EcoPark.Domain.Commons.Base;

public class UserModel(string email, string password, string firstName, string lastName)
    : BaseDataModel
{
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
}