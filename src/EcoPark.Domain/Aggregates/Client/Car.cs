namespace EcoPark.Domain.Aggregates.Client;

public class Car(CarModel carModel)
{
    public Guid Id { get; private set; } = carModel.Id;
    public string Plate { get; private set; } = carModel.Plate;
    public ECarType Type { get; private set; } = carModel.Type;
    public string Model { get; private set; } = carModel.Model;
    public string Color { get; private set; } = carModel.Color;
    public string Brand { get; private set; } = carModel.Brand;
    public int Year { get; private set; } = carModel.Year;

    public void UpdatePlate(string? plate)
    {
        if (!string.IsNullOrWhiteSpace(plate) && !plate.Equals(Plate, StringComparison.InvariantCultureIgnoreCase))
            Plate = plate;
    }

    public void UpdateType(ECarType? type)
    {
        if(type != null && type != Type) 
            Type = type.Value;
    }

    public void UpdateModel(string? model)
    {
        if(!string.IsNullOrWhiteSpace(model) && !model.Equals(Model, StringComparison.InvariantCultureIgnoreCase)) 
            Model = model;
    }

    public void UpdateColor(string? color)
    {
        if (!string.IsNullOrWhiteSpace(color) && !color.Equals(Color, StringComparison.InvariantCultureIgnoreCase))
            Color = color;
    }

    public void UpdateBrand(string? brand)
    {
        if (!string.IsNullOrWhiteSpace(brand) && !brand.Equals(Brand, StringComparison.InvariantCultureIgnoreCase))
            Brand = brand;
    }

    public void UpdateYear(int? year)
    {
        if (year != null && Year != year)
            Year = year.Value;
    }
}