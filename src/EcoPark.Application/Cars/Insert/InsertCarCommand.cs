namespace EcoPark.Application.Cars.Insert;

public class InsertCarCommand(string plate, ECarType type, string model, string brand, string color, int year) : ICommand
{
    public string Plate { get; private set; } = plate;
    public ECarType Type { get; private set; } = type;
    public string Model { get; private set; } = model;
    public string Brand { get; private set; } = brand;
    public string Color { get; private set; } = color;
    public int Year { get; private set; } = year;

    public CarModel ToModel(Guid clientId)
    {
        return new(clientId, Plate, Type, Model, Color, Brand, Year);
    }

    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}