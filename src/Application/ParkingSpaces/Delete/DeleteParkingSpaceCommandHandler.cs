namespace Application.ParkingSpaces.Delete;

public class DeleteParkingSpaceCommandHandler : IHandler<Guid, bool>
{
    public async Task<bool> HandleAsync(Guid command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}