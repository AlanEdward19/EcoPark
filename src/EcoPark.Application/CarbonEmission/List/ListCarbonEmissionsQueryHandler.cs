using EcoPark.Application.CarbonEmission.Models;

namespace EcoPark.Application.CarbonEmission.List;

public class ListCarbonEmissionsQueryHandler(IRepository<CarbonEmissionModel> repository) : IHandler<ListCarbonEmissionsQuery, IEnumerable<CarbonEmissionViewModel>>
{
    public async Task<IEnumerable<CarbonEmissionViewModel>> HandleAsync(ListCarbonEmissionsQuery command, CancellationToken cancellationToken)
    {
        var carbonEmissions = await repository.ListAsync(command, cancellationToken);

        if (carbonEmissions == null || !carbonEmissions.Any())
            return Enumerable.Empty<CarbonEmissionViewModel>();

        List<CarbonEmissionViewModel> result = new(carbonEmissions.Count());

        foreach (var carbonEmission in carbonEmissions)
        {
            CarbonEmissionViewModel model = new(carbonEmission.Reservation.ParkingSpace.LocationId,
                carbonEmission.Forecast, carbonEmission.Emission,
                carbonEmission.Inhibition);

            result.Add(model);
        }

        return result;
    }
}