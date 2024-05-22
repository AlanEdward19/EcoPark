namespace EcoPark.Application.Employees.Update;

public class UpdateEmployeeCommandHandler(IRepository<EmployeeModel> repository) : IHandler<UpdateEmployeeCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result = new(EOperationStatus.Failed, "AAAAA");

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

                    result = new DatabaseOperationResponseViewModel(EOperationStatus.Successful, "Employee updated successfully");

                    break;

                case EOperationStatus.NotFound:
                    result = new DatabaseOperationResponseViewModel(EOperationStatus.NotFound, "Employee not found");
                    break;

                case EOperationStatus.NotAuthorized:
                    result = new DatabaseOperationResponseViewModel(EOperationStatus.NotAuthorized, "You have no permission to update this employee");
                    break;

                case EOperationStatus.Failed:
                    result = new DatabaseOperationResponseViewModel(EOperationStatus.Failed, "E-mail is not available");
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