namespace EcoPark.Application.Employees.Insert;

public class InsertEmployeeCommandHandler(IRepository<EmployeeModel> repository) : IHandler<InsertEmployeeCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertEmployeeCommand command, CancellationToken cancellationToken)
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

                result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Successful, "Employee was inserted successfully!");
            }
            else
            {
                await repository.UnitOfWork.RollbackAsync(cancellationToken);
                result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, "Employee was not inserted!");
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