namespace EcoPark.Application.Employees.List;

public class ListEmployeesQueryHandler(IRepository<EmployeeModel> repository) : IHandler<ListEmployeesQuery, IEnumerable<EmployeeViewModel>>
{
    public async Task<IEnumerable<EmployeeViewModel>> HandleAsync(ListEmployeesQuery command, CancellationToken cancellationToken)
    {
        var employees = await repository.ListAsync(command, cancellationToken);

        List<EmployeeViewModel> result = new(employees.Count());

        foreach (var employeeModel in employees)
        {
            EmployeeViewModel employee = new(employeeModel.Id, employeeModel.Credentials.Email, employeeModel.Credentials.FirstName, 
                employeeModel.Credentials.LastName, employeeModel.Credentials.UserType, employeeModel.Credentials.Image);

            result.Add(employee);
        }

        return result;
    }
}