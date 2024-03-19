namespace Application.ParkingSpaces.Get;

public class GetParkingSpaceQueryHandler : IHandler<Guid, ParkingSpace>
{
    public async Task<ParkingSpace> HandleAsync(Guid command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}