namespace EcoPark.Application.Clients.Delete;

public class DeleteClientCommandHandler : IHandler<DeleteClientCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(DeleteClientCommand command, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}