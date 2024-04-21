namespace EcoPark.Application.Employees.List;

public class ListEmployeesQuery : IQuery
{
    public IEnumerable<Guid>? EmployeeIds { get; set; }
}