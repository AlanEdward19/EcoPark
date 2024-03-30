namespace EcoPark.Application.Employees.Insert;

public class InsertEmployeeCommandHandler : IHandler<InsertEmployeeCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertEmployeeCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}