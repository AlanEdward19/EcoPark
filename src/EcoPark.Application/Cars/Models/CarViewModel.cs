namespace EcoPark.Application.Cars.Models;

public class CarViewModel(Guid id, string plate, ECarType type, string brand, string model, string color, int year, EFuelType fuelType, double fuelConsumptionPerLiter)
{
    public Guid Id { get; private set; } = id;
    public string Plate { get; private set; } = plate;
    public string Type { get; private set; } = type.ToString();
    public string Brand { get; private set; } = brand;
    public string Model { get; private set; } = model;
    public string Color { get; private set; } = color;
    public int Year { get; private set; } = year;
    public string FuelType { get; private set; } = fuelType.ToString();
    public double FuelConsumptionPerLiter { get; private set; } = fuelConsumptionPerLiter;
}