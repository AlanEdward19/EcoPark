using EcoPark.Application.Clients.Delete;
using EcoPark.Application.Clients.Get;
using EcoPark.Application.Clients.Insert;
using EcoPark.Application.Clients.List;
using EcoPark.Application.Clients.Update;
using EcoPark.Domain.Interfaces.Providers;
using EcoPark.Domain.ValueObjects;
using EcoPark.Infrastructure.Migrations;
using EcoPark.Infrastructure.Providers;
using Microsoft.SqlServer.Server;
using static System.Net.Mime.MediaTypeNames;

namespace EcoPark.Infrastructure.Repositories;

public class ClientRepository(DatabaseDbContext databaseDbContext, IAuthenticationService authenticationService, IUnitOfWork unitOfWork, IStorageProvider storageProvider)
    : IAggregateRepository<ClientModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        if (command.RequestUserInfo.UserType == EUserType.PlataformAdministrator)
            return true;

        ClientModel? clientModel = null;

        switch (command)
        {
            case UpdateClientCommand updateCommand:

                clientModel = await databaseDbContext.Clients
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(
                        e => e.Credentials.Email.Equals(updateCommand.RequestUserInfo.Email) &&
                             e.Id == updateCommand.ClientId, cancellationToken);
                break;

            case DeleteClientCommand deleteCommand:
                clientModel = await databaseDbContext.Clients
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(
                        e => e.Credentials.Email.Equals(deleteCommand.RequestUserInfo.Email) &&
                             e.Id == deleteCommand.Id, cancellationToken);
                break;
        }

        return clientModel != null;
    }

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertClientCommand;
        string? blobFileName;

        if (parsedCommand.Image != null)
        {
            Guid imageId = Guid.NewGuid();
            string format = parsedCommand.ImageFileName.Split('.').Last();
            blobFileName = $"{imageId}.{format}";

            await storageProvider.WriteBlobAsync(parsedCommand.Image, blobFileName, "profiles");
        }
        else
            blobFileName = null;

        ClientModel clientModel = parsedCommand.ToModel(authenticationService, blobFileName);

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

            if (parsedCommand.Image != null)
            {
                string format = parsedCommand.ImageFileName!.Split('.').Last();
                string blobName;

                string newFileFormat = parsedCommand.ImageFileName!.Split('.').Last();

                if (string.IsNullOrWhiteSpace(clientModel.Credentials.Image))
                    blobName = $"{Guid.NewGuid()}.{format}";

                else
                {
                    blobName = clientModel.Credentials.Image;
                    var oldFileFormat = clientModel.Credentials.Image!.Split('.').Last();

                    if (!newFileFormat.Equals(oldFileFormat))
                    {
                        string oldFileName = clientModel.Credentials.Image.Split(".").First();
                        blobName = $"{oldFileName}.{newFileFormat}";
                        clientAggregate.UpdateImage(blobName);
                    }

                    await storageProvider.DeleteBlobAsync(clientModel.Credentials.Image, "profiles");
                }

                await storageProvider.WriteBlobAsync(parsedCommand.Image, blobName, "profiles");
            }

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