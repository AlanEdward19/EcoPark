namespace EcoPark.Application.Reservations.Update;

public class UpdateReservationCommandHandler : IHandler<UpdateReservationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateReservationCommand command, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}