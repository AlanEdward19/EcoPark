namespace EcoPark.Application.Reservations.Delete;

public class DeleteReservationCommandHandler : IHandler<DeleteReservationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(DeleteReservationCommand command, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}