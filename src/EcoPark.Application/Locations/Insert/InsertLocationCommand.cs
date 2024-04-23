namespace EcoPark.Application.Locations.Insert;

public class InsertLocationCommand(string? name, string? address) : ICommand
{
    public string? Name { get; private set; } = name;
    public string? Address { get; private set; } = address;

    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}