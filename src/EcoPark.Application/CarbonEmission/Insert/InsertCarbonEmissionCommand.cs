namespace EcoPark.Application.CarbonEmission.Insert;

public class InsertCarbonEmissionCommand(double? forecast, double? emission, double? inhibition) : ICommand
{
    public double? Forecast { get; private set; } = forecast;
    public double? Emission { get; private set; } = emission;
    public double? Inhibition { get; private set; } = inhibition;
    public Guid ReservationId { get; private set; }

    public void SetReservationId(Guid reservationId)
    {
        ReservationId = reservationId;
    }

    public CarbonEmissionModel ToModel(Guid clientId)
    {
        return new(clientId, ReservationId, Forecast!.Value, Emission!.Value, Inhibition!.Value);
    }

    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}