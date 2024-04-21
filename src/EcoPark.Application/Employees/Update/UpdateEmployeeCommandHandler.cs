namespace EcoPark.Application.Employees.Update;

public class UpdateEmployeeCommandHandler(IRepository<EmployeeModel> repository) : IHandler<UpdateEmployeeCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateEmployeeCommand command, CancellationToken cancellationToken)
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

                result = new("Patch", EOperationStatus.Successful, "Employee updated successfully");
            }
            else
            {
                await repository.UnitOfWork.RollbackAsync(cancellationToken);
                result = new("Patch", EOperationStatus.Failed, "No Employee were found with this id");
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