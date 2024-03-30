namespace EcoPark.Domain.Aggregates.Client;

public class Car
{
    public Guid Id { get; private set; }
    public string Plate { get; private set; }
    public ECarType Type { get; private set; }
    public string Model { get; private set; }
    public string Color { get; private set; }
    public string Brand { get; private set; }
    public int Year { get; private set; }

    public void UpdatePlate(string plate) => Plate = plate;

    public void UpdateType(ECarType type) => Type = type;

    public void UpdateModel(string model) => Model = model;

    public void UpdateColor(string color) => Color = color;

    public void UpdateBrand(string brand) => Brand = brand;

    public void UpdateYear(int year) => Year = year;
}