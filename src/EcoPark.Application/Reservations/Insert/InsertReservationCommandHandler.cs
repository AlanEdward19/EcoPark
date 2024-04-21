namespace EcoPark.Application.Reservations.Insert;

public class InsertReservationCommandHandler(IRepository<ReservationModel> repository) : IHandler<InsertReservationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertReservationCommand command, 
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            await repository.UnitOfWork.StartAsync(cancellationToken);

            var databaseOperationResult = await repository.AddAsync(command, cancellationToken);

            if (databaseOperationResult)
            {
                await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                await repository.UnitOfWork.CommitAsync(cancellationToken);

                result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Successful,
                    "Reservation was inserted successfully!");
            }
            else
            {
                await repository.UnitOfWork.RollbackAsync(cancellationToken);
                result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, "Reservation was not inserted!");
            }
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}