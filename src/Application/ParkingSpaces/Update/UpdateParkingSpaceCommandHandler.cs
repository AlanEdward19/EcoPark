namespace Application.ParkingSpaces.Update;

public class UpdateParkingSpaceCommandHandler : IHandler<UpdateParkingSpaceCommand, Guid>
{
    public async Task<Guid> HandleAsync(UpdateParkingSpaceCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}