namespace EcoPark.Domain.Interfaces.Services;

public interface IHandler<TCommand, TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}