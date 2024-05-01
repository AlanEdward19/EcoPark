namespace EcoPark.Application.Employees.Insert.GroupAccess;

public class InsertEmployeeGroupAccessCommandHandler(IRepository<EmployeeModel> repository) : IHandler<InsertEmployeeGroupAccessCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertEmployeeGroupAccessCommand command, CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            if (await repository.CheckChangePermissionAsync(command, CancellationToken.None))
            {
                await repository.UnitOfWork.StartAsync(cancellationToken);

                var databaseOperationResult = await repository.AddAsync(command, cancellationToken);

                if (databaseOperationResult)
                {
                    await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    await repository.UnitOfWork.CommitAsync(cancellationToken);

                    result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Successful, "Employee received access to location successfully");
                }
                else
                {
                    await repository.UnitOfWork.RollbackAsync(cancellationToken);
                    result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, "Employee didn't receive access to location");
                }
            }
            else
                result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, "You don't have permission to give access to this location");
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}