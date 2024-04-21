namespace EcoPark.Application.ParkingSpaces.Delete;

public class DeleteParkingSpaceCommandHandler(IAggregateRepository<ParkingSpaceModel> repository) : IHandler<DeleteParkingSpaceCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(DeleteParkingSpaceCommand command,
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            await repository.UnitOfWork.StartAsync(cancellationToken);

            var databaseOperationResult = await repository.DeleteAsync(command, cancellationToken);

            if (databaseOperationResult)
            {
                await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                await repository.UnitOfWork.CommitAsync(cancellationToken);

                result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Successful,
                    "Parking space was deleted successfully!");
            }
            else
            {
                await repository.UnitOfWork.RollbackAsync(cancellationToken);
                result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed,
                    "No Parking space were found with this id");
            }
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}