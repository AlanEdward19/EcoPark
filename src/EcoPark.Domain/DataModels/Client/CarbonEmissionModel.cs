namespace EcoPark.Domain.DataModels.Client;

public class CarbonEmissionModel(Guid clientId, Guid reservationId, double forecast, double emission, double inhibition) : BaseDataModel
{
    public Guid ClientId { get; set; } = clientId;
    public Guid ReservationId { get; set; } = reservationId;
    public double Forecast { get; set; } = forecast;
    public double Emission { get; set; } = emission;
    public double Inhibition { get; set; } = inhibition;
    public bool IsConfirmed { get; set; } = false;

    [ForeignKey(nameof(ClientId))]
    public virtual ClientModel Client { get; set; }

    [ForeignKey(nameof(ReservationId))]
    public virtual ReservationModel Reservation { get; set; }
}