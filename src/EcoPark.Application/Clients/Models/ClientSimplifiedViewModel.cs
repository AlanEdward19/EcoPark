namespace EcoPark.Application.Clients.Models;

public class ClientSimplifiedViewModel(Guid id, string email, string firstName, string lastName)
{
    public Guid Id { get; private set; } = id;
    public string Email { get; private set; } = email;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
}