namespace EcoPark.Application.Clients.Update;

public class UpdateClientCommandHandler(IRepository<ClientModel> repository) : IHandler<UpdateClientCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateClientCommand command,
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result = new(EOperationStatus.Failed, "");

        try
        {
            EOperationStatus status = await repository.CheckChangePermissionAsync(command, cancellationToken);

            switch (status)
            {
                case EOperationStatus.Successful:
                    await repository.UnitOfWork.StartAsync(cancellationToken);

                    await repository.UpdateAsync(command, cancellationToken);

                    await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    await repository.UnitOfWork.CommitAsync(cancellationToken);

                    result = new(EOperationStatus.Successful, "Client updated successfully");

                    break;

                case EOperationStatus.Failed:
                    result = new DatabaseOperationResponseViewModel(EOperationStatus.Failed, "E-mail is not available");
                    break;

                case EOperationStatus.NotAuthorized:
                    result = new(EOperationStatus.NotAuthorized, "You have no permission to update this client");
                    break;

                case EOperationStatus.NotFound:
                    result = new(EOperationStatus.NotFound, "No client were found with this id");
                    break;
            }

        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new(EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}