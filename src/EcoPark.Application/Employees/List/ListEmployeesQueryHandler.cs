namespace EcoPark.Application.Employees.List;

public class ListEmployeesQueryHandler : IHandler<ListEmployeesQuery, IEnumerable<EmployeeViewModel>>
{
    public async Task<IEnumerable<EmployeeViewModel>> HandleAsync(ListEmployeesQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}