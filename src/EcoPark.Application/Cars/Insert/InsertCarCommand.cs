﻿namespace EcoPark.Application.Cars.Insert;

public class InsertCarCommand(string? plate, ECarType? type, string? model, string? brand, string? color, 
    int? year, EFuelType? fuelType, double? fuelConsumptionPerLiter) : ICommand
{
    public string? Plate { get; private set; } = plate;
    public ECarType? Type { get; private set; } = type;
    public string? Model { get; private set; } = model;
    public string? Brand { get; private set; } = brand;
    public string? Color { get; private set; } = color;
    public int? Year { get; private set; } = year;
    public EFuelType? FuelType { get; private set; } = fuelType;
    public double? FuelConsumptionPerLiter { get; private set; } = fuelConsumptionPerLiter;

    public CarModel ToModel(Guid clientId)
    {
        return new(clientId, Plate!, Type!.Value, Model!, Color!, Brand!, Year!.Value, FuelType!.Value,
            FuelConsumptionPerLiter!.Value);
    }

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}