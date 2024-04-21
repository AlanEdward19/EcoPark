namespace EcoPark.Application.Clients.Update;

public class UpdateClientCommandHandler(IAggregateRepository<ClientModel> repository) : IHandler<UpdateClientCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateClientCommand command, 
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            await repository.UnitOfWork.StartAsync(cancellationToken);

            var databaseOperationResult = await repository.UpdateAsync(command, cancellationToken);

            if (databaseOperationResult)
            {
                await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                await repository.UnitOfWork.CommitAsync(cancellationToken);

                result = new("Patch", EOperationStatus.Successful, "Client updated successfully");
            }
            else
            {
                await repository.UnitOfWork.RollbackAsync(cancellationToken);
                result = new("Patch", EOperationStatus.Failed, "No Client were found with this id");
            }
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new("Patch", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}