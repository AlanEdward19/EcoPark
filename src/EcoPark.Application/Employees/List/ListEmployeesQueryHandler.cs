namespace EcoPark.Application.Employees.List;

public class ListEmployeesQueryHandler(DatabaseDbContext databaseDbContext) : IHandler<ListEmployeesQuery, IEnumerable<EmployeeViewModel>>
{
    public async Task<IEnumerable<EmployeeViewModel>> HandleAsync(ListEmployeesQuery command, CancellationToken cancellationToken)
    {
        bool hasEmployeeIds = command.EmployeeIds != null && command.EmployeeIds.Any();

        List<EmployeeViewModel> result = hasEmployeeIds
            ? new(command.EmployeeIds.Count())
            : new(100);

        IQueryable<EmployeeModel> query = databaseDbContext.Employees;

        if(hasEmployeeIds)
            query = query.Where(e => command.EmployeeIds!.Contains(e.Id));

        List<EmployeeModel> employeeModels = await query.ToListAsync(cancellationToken);

        foreach (var employeeModel in employeeModels)
        {
            EmployeeViewModel employee = new(employeeModel.Email, employeeModel.FirstName, employeeModel.LastName, employeeModel.UserType);

            result.Add(employee);
        }

        return result;
    }
}