namespace EcoPark.Application.Locations.Update;

public class UpdateLocationCommand(string name, string address) : ICommand
{
    public Guid LocationId { get; private set; }
    public string Name { get; private set; } = name;
    public string Address { get; private set; } = address;

    public void SetLocationId(Guid locationId) => LocationId = locationId;

    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}