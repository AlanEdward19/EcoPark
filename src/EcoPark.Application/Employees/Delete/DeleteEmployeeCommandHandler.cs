namespace EcoPark.Application.Employees.Delete;

public class DeleteEmployeeCommandHandler(DatabaseDbContext databaseDbContext) : IHandler<DeleteEmployeeCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            EmployeeModel? employeeModel = await databaseDbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == command.Id, cancellationToken);

            if (employeeModel != null)
            {
                databaseDbContext.Employees.Remove(employeeModel);

                await databaseDbContext.SaveChangesAsync(cancellationToken);

                result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Successful, "Employee was deleted successfully!");
            }
            else
                result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed, "No Employee were found with this id");

        }
        catch (Exception e)
        {
            result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}