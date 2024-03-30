namespace EcoPark.Application.Clients.Insert;

public class InsertClientCommandHandler : IHandler<InsertClientCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertClientCommand command, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}