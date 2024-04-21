namespace EcoPark.Application.Locations.Delete;

public class DeleteLocationCommandHandler(IAggregateRepository<LocationModel> repository): IHandler<DeleteLocationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(DeleteLocationCommand command,
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
                    "Location was deleted successfully!");
            }
            else
            {
                await repository.UnitOfWork.RollbackAsync(cancellationToken);
                result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed,
                    "No Location were found with this id");
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