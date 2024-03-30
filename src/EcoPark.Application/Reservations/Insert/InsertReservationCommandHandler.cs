namespace EcoPark.Application.Reservations.Insert;

public class InsertReservationCommandHandler : IHandler<InsertReservationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertReservationCommand command, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}