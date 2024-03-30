namespace EcoPark.Application.Clients.Get;

public class GetClientQueryHandler : IHandler<GetClientQuery, ClientSimplifiedViewModel>
{
    public async Task<ClientSimplifiedViewModel> HandleAsync(GetClientQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}