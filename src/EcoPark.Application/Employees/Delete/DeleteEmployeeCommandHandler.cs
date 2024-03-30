namespace EcoPark.Application.Employees.Delete;

public class DeleteEmployeeCommandHandler : IHandler<DeleteEmployeeCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}