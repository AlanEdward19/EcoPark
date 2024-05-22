namespace EcoPark.Application.Cars.Update;

public class UpdateCarCommand(string? plate, ECarType? type, string? model, string? brand, string? color, 
        int? year, EFuelType? fuelType, double? fuelConsumptionPerLiter) : ICommand
{
    public Guid CarId { get; private set; }
    public string? Plate { get; private set; } = plate;
    public ECarType? Type { get; private set; } = type;
    public string? Model { get; private set; } = model;
    public string? Brand { get; private set; } = brand;
    public string? Color { get; private set; } = color;
    public int? Year { get; private set; } = year;
    public EFuelType? FuelType { get; private set; } = fuelType;
    public double? FuelConsumptionPerLiter { get; private set; } = fuelConsumptionPerLiter;

    public void SetCarId(Guid carId)
    {
        CarId = carId;
    }

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}