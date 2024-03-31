namespace EcoPark.Domain.Commons.Base;

public class UserModel(string email, string password, string firstName, string lastName)
    : BaseDataModel
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
}