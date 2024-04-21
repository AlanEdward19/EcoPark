using EcoPark.Application.Clients.Models;

namespace EcoPark.Infrastructure.Repositories;

public class ClientRepository : IRepository<ClientSimplifiedViewModel>
{
    public IUnitOfWork UnitOfWork { get; }
    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ClientSimplifiedViewModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ClientSimplifiedViewModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}