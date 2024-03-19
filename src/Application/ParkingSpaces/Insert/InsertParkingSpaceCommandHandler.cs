namespace Application.ParkingSpaces.Insert;

public class InsertParkingSpaceCommandHandler : IHandler<InsertParkingSpaceCommand, Guid>
{
    public async Task<Guid> HandleAsync(InsertParkingSpaceCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}