namespace EcoPark.Application.Employees.Update;

public class UpdateEmployeeCommandHandler : IHandler<UpdateEmployeeCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}