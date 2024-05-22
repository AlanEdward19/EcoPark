namespace EcoPark.Domain.Interfaces.Database;

public interface IRepository<T>
{
    IUnitOfWork UnitOfWork { get; }

    Task<EOperationStatus> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken);
    Task AddAsync(ICommand command, CancellationToken cancellationToken);
    Task UpdateAsync(ICommand command, CancellationToken cancellationToken);
    Task DeleteAsync(ICommand command, CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(IQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<T>> ListAsync(IQuery query, CancellationToken cancellationToken);
}