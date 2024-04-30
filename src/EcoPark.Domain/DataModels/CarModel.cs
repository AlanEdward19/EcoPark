using EcoPark.Domain.Aggregates.Client;

namespace EcoPark.Domain.DataModels;

public class CarModel(Guid clientId, string plate, ECarType type, string model, string color, string brand, int year)
    : BaseDataModel
{
    public Guid ClientId { get; set; } = clientId;
    public string Plate { get; set; } = plate;
    public ECarType Type { get; set; } = type;
    public string Model { get; set; } = model;
    public string Color { get; set; } = color;
    public string Brand { get; set; } = brand;
    public int Year { get; set; } = year;

    [ForeignKey(nameof(ClientId))]
    public virtual ClientModel Client { get; set; }

    public virtual ICollection<ReservationModel>? Reservations { get; set; }

    public void UpdateBasedOnValueObject(Car car)
    {
        Plate = car.Plate;
        Type = car.Type;
        Model = car.Model;
        Color = car.Color;
        Brand = car.Brand;
        Year = car.Year;
    }
}