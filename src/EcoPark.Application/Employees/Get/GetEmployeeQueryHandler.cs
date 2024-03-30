namespace EcoPark.Application.Employees.Get;

public class GetEmployeeQueryHandler : IHandler<GetEmployeeQuery, EmployeeViewModel>
{
    public async Task<EmployeeViewModel> HandleAsync(GetEmployeeQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}