﻿namespace EcoPark.Infrastructure;

public class UnitOfWork(DatabaseDbContext databaseDbContext) : IUnitOfWork
{
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken)
    {
        return await databaseDbContext.SaveChangesAsync(cancellationToken) != 0;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await databaseDbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await databaseDbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await databaseDbContext.Database.RollbackTransactionAsync(cancellationToken);
    }
}