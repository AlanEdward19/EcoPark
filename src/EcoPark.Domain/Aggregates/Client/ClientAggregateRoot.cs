namespace EcoPark.Domain.Aggregates.Client;

public class ClientAggregateRoot
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Password { get; set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? Image { get; private set; }

    public ClientAggregateRoot(Guid id, string email, string password, string firstName, string lastName, string image)
    {
        Id = id;
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Image = image;
    }

    public ClientAggregateRoot() { }

    public ClientAggregateRoot(ClientModel client)
    {
        Id = client.Credentials.Id;
        Email = client.Credentials.Email;
        Password = client.Credentials.Password;
        FirstName = client.Credentials.FirstName;
        LastName = client.Credentials.LastName;
        Image = client.Credentials.Image;
    }

    public void UpdateFirstName(string firstName)
    {
        if (!string.IsNullOrWhiteSpace(firstName) && !FirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
            FirstName = firstName;
    }

    public void UpdateLastName(string lastName)
    {
        if (!string.IsNullOrWhiteSpace(lastName) && !LastName.Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
            LastName = lastName;
    }

    public void UpdateEmail(string email)
    {
        if (!string.IsNullOrWhiteSpace(email) && !Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
            Email = email.ToLower();
    }

    public void UpdatePassword(string password)
    {
        if (!string.IsNullOrWhiteSpace(password) && !Password.Equals(password, StringComparison.InvariantCultureIgnoreCase))
            Password = password;
    }

    public void UpdateImage(string image)
    {
        if (!string.IsNullOrWhiteSpace(image) && !Image.Equals(image, StringComparison.InvariantCultureIgnoreCase))
            Image = image;
    }

    public string GetFullName() => $"{FirstName} {LastName}";
}