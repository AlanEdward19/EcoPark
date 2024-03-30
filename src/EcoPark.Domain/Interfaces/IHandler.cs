namespace EcoPark.Domain.Interfaces;

public interface IHandler<TCommand, TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}