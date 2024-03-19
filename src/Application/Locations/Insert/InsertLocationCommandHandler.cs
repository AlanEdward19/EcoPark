namespace Application.Locations.Insert;

public class InsertLocationCommandHandler : IHandler<InsertLocationCommand, Guid> //arrumar
{
    public Task<Guid> HandleAsync(InsertLocationCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}