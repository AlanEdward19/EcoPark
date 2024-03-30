namespace EcoPark.Application.Clients.Update;

public class UpdateClientCommandHandler : IHandler<UpdateClientCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateClientCommand command, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}