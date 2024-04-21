namespace EcoPark.Application.Clients.Get;

public class GetClientQueryHandler (IAggregateRepository<ClientSimplifiedViewModel> repository) : IHandler<GetClientQuery, ClientSimplifiedViewModel?>
{
    public async Task<ClientSimplifiedViewModel?> HandleAsync(GetClientQuery command, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(command, cancellationToken);
    }
}