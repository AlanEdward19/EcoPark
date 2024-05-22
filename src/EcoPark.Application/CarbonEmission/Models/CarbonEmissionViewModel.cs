namespace EcoPark.Application.CarbonEmission.Models;

public class CarbonEmissionViewModel(Guid locationId, double forecast, double emission, double inhibition)
{
    public Guid LocationId { get; private set; } = locationId;
    public double Forecast { get; private set; } = forecast;
    public double Emission { get; private set; } = emission;
    public double Inhibition { get; private set; } = inhibition;
}