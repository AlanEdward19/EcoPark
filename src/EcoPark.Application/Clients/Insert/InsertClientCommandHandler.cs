namespace EcoPark.Application.Clients.Insert;

public class InsertClientCommandHandler(IRepository<ClientModel> repository) : IHandler<InsertClientCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertClientCommand command, 
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;
        try
        {
            await repository.UnitOfWork.StartAsync(cancellationToken);

            await repository.AddAsync(command, cancellationToken);

            await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            await repository.UnitOfWork.CommitAsync(cancellationToken);

            result = new DatabaseOperationResponseViewModel(EOperationStatus.Successful, "Client was inserted successfully!");

        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new DatabaseOperationResponseViewModel( EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}