namespace EcoPark.Application.Employees.Get;

public class GetEmployeeQueryHandler(DatabaseDbContext databaseDbContext) : IHandler<GetEmployeeQuery, EmployeeViewModel?>
{
    public async Task<EmployeeViewModel?> HandleAsync(GetEmployeeQuery command, CancellationToken cancellationToken)
    {
        EmployeeViewModel? result = null;

        EmployeeModel? employeeModel =
            await databaseDbContext.Employees.FirstOrDefaultAsync(e => e.Id == command.EmployeeId, cancellationToken);

        if (employeeModel != null)
        {
            result = new EmployeeViewModel(employeeModel.Email, employeeModel.FirstName, employeeModel.LastName,
                employeeModel.UserType);
        }

        return result;
    }
}