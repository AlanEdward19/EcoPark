namespace EcoPark.Application.Employees.Insert;

public class InsertEmployeeCommandHandler(DatabaseDbContext databaseContext, IAuthenticationService authenticationService) 
    : IHandler<InsertEmployeeCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertEmployeeCommand command, CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            EmployeeModel employeeModel = command.ToModel(authenticationService);

            await databaseContext.Employees.AddAsync(employeeModel, cancellationToken);

            await databaseContext.SaveChangesAsync(cancellationToken);

            result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Successful, "Employee was inserted successfully!");
        }
        catch (Exception e)
        {
            result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}