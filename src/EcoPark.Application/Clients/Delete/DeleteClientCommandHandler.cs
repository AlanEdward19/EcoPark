namespace EcoPark.Application.Clients.Delete;

public class DeleteClientCommandHandler(IAggregateRepository<ClientModel> repository) : IHandler<DeleteClientCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(DeleteClientCommand command, 
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;
        try
        {
            if (await repository.CheckChangePermissionAsync(command, cancellationToken))
            {
                await repository.UnitOfWork.StartAsync(cancellationToken);

                var databaseOperationResult = await repository.DeleteAsync(command, cancellationToken);

                if (databaseOperationResult)
                {
                    await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    await repository.UnitOfWork.CommitAsync(cancellationToken);

                    result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Successful,
                        "Client was deleted successfully!");
                }
                else
                {
                    await repository.UnitOfWork.RollbackAsync(cancellationToken);
                    result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed,
                        "No Client were found with this id");
                }
            }
            else
                result = new("Delete", EOperationStatus.NotAuthorized, "You have no permission to delete this client");

        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}