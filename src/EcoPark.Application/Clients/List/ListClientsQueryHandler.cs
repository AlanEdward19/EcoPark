namespace EcoPark.Application.Clients.List;

public class ListClientsQueryHandler(IAggregateRepository<ClientSimplifiedViewModel> repository) : IHandler<ListClientsQuery, IEnumerable<ClientSimplifiedViewModel>>
{
    public async Task<IEnumerable<ClientSimplifiedViewModel>> HandleAsync(ListClientsQuery command, CancellationToken cancellationToken)
    {
        return await repository.ListAsync(command, cancellationToken);
    }
}