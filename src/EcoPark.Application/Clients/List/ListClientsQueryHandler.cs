namespace EcoPark.Application.Clients.List;

public class ListClientsQueryHandler : IHandler<ListClientsQuery, IEnumerable<ClientSimplifiedViewModel>>
{
    public async Task<IEnumerable<ClientSimplifiedViewModel>> HandleAsync(ListClientsQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}