namespace Application.Locations.List;

public class ListLocationsQueryHandler : IHandler<IEnumerable<Guid>?, IEnumerable<Location>>
{
    public async Task<IEnumerable<Location>> HandleAsync(IEnumerable<Guid>? command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}