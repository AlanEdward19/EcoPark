namespace EcoPark.Application.Clients.Models;

public class ClientSimplifiedViewModel(string email, string firstName, string lastName)
{
    public string Email { get; private set; } = email;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
}