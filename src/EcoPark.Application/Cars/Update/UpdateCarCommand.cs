namespace EcoPark.Application.Cars.Update;

public class UpdateCarCommand(string? plate, ECarType? type, string? model, string? brand, string? color, int? year)
    : ICommand
{
    public Guid CarId { get; private set; }
    public string? Plate { get; private set; } = plate;
    public ECarType? Type { get; private set; } = type;
    public string? Model { get; private set; } = model;
    public string? Brand { get; private set; } = brand;
    public string? Color { get; private set; } = color;
    public int? Year { get; private set; } = year;

    public void SetCarId(Guid carId)
    {
        CarId = carId;
    }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}