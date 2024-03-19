namespace Application.Reservations.Update;

public class UpdateReservationCommandHandler : IHandler<UpdateReservationCommand, Guid>
{
    public async Task<Guid> HandleAsync(UpdateReservationCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}