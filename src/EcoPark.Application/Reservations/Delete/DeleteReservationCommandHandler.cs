namespace EcoPark.Application.Reservations.Delete;

public class DeleteReservationCommandHandler(IRepository<ReservationModel> repository) : IHandler<DeleteReservationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(DeleteReservationCommand command,
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result = new(EOperationStatus.Failed, "AAAAA");

        try
        {
            EOperationStatus status = await repository.CheckChangePermissionAsync(command, cancellationToken);

            switch (status)
            {
                case EOperationStatus.Successful:
                    await repository.UnitOfWork.StartAsync(cancellationToken);

                    await repository.DeleteAsync(command, cancellationToken);

                    await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    await repository.UnitOfWork.CommitAsync(cancellationToken);

                    result = new DatabaseOperationResponseViewModel(EOperationStatus.Successful,
                        "Reservation was deleted successfully!");

                    break;

                case EOperationStatus.NotAuthorized:
                    result = new DatabaseOperationResponseViewModel(EOperationStatus.NotAuthorized,
                        "You have no permission to delete this reservation");
                    break;

                case EOperationStatus.Failed:
                    break;

                case EOperationStatus.NotFound:
                    result = new DatabaseOperationResponseViewModel(EOperationStatus.NotFound,
                        "Reservation not found, check input data");
                    break;
            }
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new DatabaseOperationResponseViewModel(EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}