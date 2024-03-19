namespace Application.Locations.Delete;

public class DeleteLocationCommandHandler : IHandler<Guid, bool>
{
    public async Task<bool> HandleAsync(Guid command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}