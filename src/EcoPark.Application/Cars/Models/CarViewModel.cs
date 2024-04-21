namespace EcoPark.Application.Cars.Models;

public class CarViewModel(string plate, ECarType type, string brand, string model, string color, int year)
{
    public string Plate { get; private set; } = plate;
    public string Type { get; private set; } = type.ToString();
    public string Brand { get; private set; } = brand;
    public string Model { get; private set; } = model;
    public string Color { get; private set; } = color;
    public int Year { get; private set; } = year;
}