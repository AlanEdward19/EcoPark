﻿using EcoPark.Application.Clients.Delete;
using EcoPark.Application.Clients.Get;
using EcoPark.Application.Clients.Insert;
using EcoPark.Application.Clients.List;
using EcoPark.Application.Clients.Update;

namespace EcoPark.Infrastructure.Repositories;

public class ClientRepository(DatabaseDbContext databaseDbContext, IAuthenticationService authenticationService, IUnitOfWork unitOfWork)
    : IAggregateRepository<ClientModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        if (command.RequestUserInfo.UserType == EUserType.PlataformAdministrator)
            return true;

        ClientModel? clientModel = null;

        switch (command.GetType().Name)
        {
            case nameof(UpdateClientCommand):
                var parsedUpdateCommand = command as UpdateClientCommand;
                clientModel = await databaseDbContext.Clients
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(
                        e => e.Credentials.Email.Equals(parsedUpdateCommand.RequestUserInfo.Email) &&
                             e.Id == parsedUpdateCommand.ClientId, cancellationToken);
                break;

            case nameof(DeleteClientCommand):
                var parsedDeleteCommand = command as DeleteClientCommand;
                clientModel = await databaseDbContext.Clients
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(
                        e => e.Credentials.Email.Equals(parsedDeleteCommand.RequestUserInfo.Email) &&
                             e.Id == parsedDeleteCommand.Id, cancellationToken);
                break;
        }

        return clientModel != null;
    }

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertClientCommand;

        ClientModel clientModel = parsedCommand.ToModel(authenticationService);

        await databaseDbContext.Clients.AddAsync(clientModel, cancellationToken);

        return true;
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UpdateClientCommand;

        ClientModel? clientModel = await databaseDbContext.Clients
            .Include(x => x.Credentials)
            .FirstOrDefaultAsync(e => e.Id == parsedCommand.ClientId, cancellationToken);

        if (clientModel != null)
        {
            ClientAggregateRoot clientAggregate = new(clientModel);

            clientAggregate.UpdateEmail(parsedCommand.Email);
            clientAggregate.UpdatePassword(authenticationService.ComputeSha256Hash(parsedCommand.Password!));
            clientAggregate.UpdateFirstName(parsedCommand.FirstName);
            clientAggregate.UpdateLastName(parsedCommand.LastName);

            clientModel.UpdateBasedOnAggregate(clientAggregate);

            databaseDbContext.Clients.Update(clientModel);

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as DeleteClientCommand;

        ClientModel? clientModel = await databaseDbContext.Clients
            .Include(x => x.Credentials)
            .FirstOrDefaultAsync(e => e.Id == parsedCommand.Id, cancellationToken);

        if (clientModel == null) return false;

        databaseDbContext.Clients.Remove(clientModel);
        databaseDbContext.Credentials.Remove(clientModel.Credentials);
        return true;
    }

    public async Task<ClientModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as GetClientQuery;

        IQueryable<ClientModel> databaseQuery =
            databaseDbContext.Clients
                .AsNoTracking()
                .AsQueryable();

        if (parsedQuery.IncludeCars)
            databaseQuery = databaseQuery.Include(c => c.Cars);

        return await databaseQuery.FirstOrDefaultAsync(c => c.Id == parsedQuery.ClientId, cancellationToken);
    }

    public async Task<IEnumerable<ClientModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListClientsQuery;

        bool hasClientIds = parsedQuery.ClientIds != null && parsedQuery.ClientIds.Any();

        IQueryable<ClientModel> databaseQuery =
            databaseDbContext.Clients.AsNoTracking().AsQueryable();

        if (parsedQuery.IncludeCars)
            databaseQuery = databaseQuery.Include(c => c.Cars);

        if (hasClientIds)
            databaseQuery = databaseQuery.Where(c => parsedQuery.ClientIds!.Contains(c.Id));

        return await databaseQuery.ToListAsync(cancellationToken);
    }
}