using EcoPark.Application.Locations.Delete;
using EcoPark.Application.Locations.Get;
using EcoPark.Application.Locations.Insert;
using EcoPark.Application.Locations.List;
using EcoPark.Application.Locations.Update;
using EcoPark.Domain.Aggregates.Location;

namespace EcoPark.Infrastructure.Repositories;

public class LocationRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork) : IAggregateRepository<LocationModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertLocationCommand;

        LocationModel locationModel = new(parsedCommand.Name, parsedCommand.Address);

        await databaseDbContext.Locations.AddAsync(locationModel, cancellationToken);

        return true;
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UpdateLocationCommand;

        LocationModel? locationModel = await databaseDbContext.Locations
            .FirstOrDefaultAsync(l => l.Id == parsedCommand.LocationId, cancellationToken);

        if (locationModel != null)
        {
            LocationAggregateRoot locationAggregate = new(locationModel);

            locationAggregate.UpdateName(parsedCommand.Name);
            locationAggregate.UpdateAddress(parsedCommand.Address);

            locationModel.UpdateBasedOnAggregate(locationAggregate);

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as DeleteLocationCommand;

        LocationModel? locationModel = await databaseDbContext.Locations
            .FirstOrDefaultAsync(l => l.Id == parsedCommand.Id, cancellationToken);

        if (locationModel == null) return false;

        databaseDbContext.Locations.Remove(locationModel);

        return true;
    }

    public async Task<LocationModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as GetLocationQuery;

        LocationModel? locationModel;

        IQueryable<LocationModel> databaseQuery = databaseDbContext.Locations.AsNoTracking();

        if (parsedQuery.IncludeParkingSpaces!.Value)
            locationModel = await databaseQuery.Include(l => l.ParkingSpaces)
                .FirstOrDefaultAsync(l => l.Id == parsedQuery.LocationId, cancellationToken);
        else
            locationModel = await databaseQuery.FirstOrDefaultAsync(l => l.Id == parsedQuery.LocationId, cancellationToken);

        return locationModel;
    }

    public async Task<IEnumerable<LocationModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListLocationQuery;

        bool hasLocationIds = parsedQuery.LocationIds != null && parsedQuery.LocationIds.Any();
        IQueryable<LocationModel> databaseQuery = databaseDbContext.Locations.AsNoTracking();

        if (hasLocationIds)
            databaseQuery = databaseQuery.Where(l => parsedQuery.LocationIds!.Contains(l.Id));

        if (parsedQuery.IncludeParkingSpaces!.Value)
            databaseQuery = databaseQuery.Include(l => l.ParkingSpaces);

        return await databaseQuery.ToListAsync(cancellationToken);
    }
}