namespace EcoPark.Application.Employees.Delete.GroupAccess;

public class DeleteEmployeeGroupAccessCommandHandler(IRepository<EmployeeModel> repository) : IHandler<DeleteEmployeeGroupAccessCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(DeleteEmployeeGroupAccessCommand command, CancellationToken cancellationToken)
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
                        "Employee access was deleted successfully!");
                }
                else
                {
                    await repository.UnitOfWork.RollbackAsync(cancellationToken);
                    result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed,
                        "No Access to this location were found with this id");
                }
            }
            else
                result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.NotAuthorized,
                    "You have no permission to delete this access employee");

        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}