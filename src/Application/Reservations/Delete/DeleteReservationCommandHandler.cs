namespace Application.Reservations.Delete;

public class DeleteReservationCommandHandler : IHandler<Guid, bool>
{
    public async Task<bool> HandleAsync(Guid command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}