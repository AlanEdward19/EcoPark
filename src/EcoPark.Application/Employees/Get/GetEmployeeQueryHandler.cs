namespace EcoPark.Application.Employees.Get;

public class GetEmployeeQueryHandler(IRepository<EmployeeModel> repository) : IHandler<GetEmployeeQuery, EmployeeViewModel?>
{
    public async Task<EmployeeViewModel?> HandleAsync(GetEmployeeQuery command, CancellationToken cancellationToken)
    {
        EmployeeViewModel? result = null;

        var employee =  await repository.GetByIdAsync(command, cancellationToken);

        if (employee != null)
            result = new EmployeeViewModel(employee.Email, employee.FirstName, employee.LastName,
                employee.UserType);

        return result;
    }
}