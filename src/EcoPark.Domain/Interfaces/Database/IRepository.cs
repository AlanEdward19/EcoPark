namespace EcoPark.Domain.Interfaces.Database;

public interface IRepository<T>
{
    IUnitOfWork UnitOfWork { get; }

    Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(ICommand command, CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(IQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<T>> ListAsync(IQuery query, CancellationToken cancellationToken);
}