namespace Application.Locations.Get;

public class GetLocationQueryHandler : IHandler<Guid, Location>
{
    public async Task<Location> HandleAsync(Guid command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}