namespace Application.Locations.Update;

public class UpdateLocationCommandHandler : IHandler<UpdateLocationCommand, Guid> //arrumar
{
    public async Task<Guid> HandleAsync(UpdateLocationCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}