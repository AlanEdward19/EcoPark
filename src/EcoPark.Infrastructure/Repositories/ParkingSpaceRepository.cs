using EcoPark.Application.ParkingSpaces.Delete;
using EcoPark.Application.ParkingSpaces.Get;
using EcoPark.Application.ParkingSpaces.Insert;
using EcoPark.Application.ParkingSpaces.List;
using EcoPark.Application.ParkingSpaces.Update;
using EcoPark.Domain.Aggregates.Location.ParkingSpace;

namespace EcoPark.Infrastructure.Repositories;

public class ParkingSpaceRepository(DatabaseDbContext databaseDbContext) : IAggregateRepository<ParkingSpaceModel>
{
    public IUnitOfWork UnitOfWork { get; }

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertParkingSpaceCommand;

        ParkingSpaceModel parkingSpaceModel = new(parsedCommand.LocationId!.Value, parsedCommand.Floor!.Value, parsedCommand.ParkingSpaceName!,
            parsedCommand.IsOccupied!.Value, parsedCommand.Type!.Value);

        await databaseDbContext.ParkingSpaces.AddAsync(parkingSpaceModel, cancellationToken);

        return true;
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UpdateParkingSpaceCommand;

        ParkingSpaceModel? parkingSpaceModel = await databaseDbContext.ParkingSpaces
            .FirstOrDefaultAsync(p => p.Id == parsedCommand.ParkingSpaceId, cancellationToken);

        if (parkingSpaceModel != null)
        {
            ParkingSpaceAggregate parkingSpaceAggregate = new(parkingSpaceModel);

            parkingSpaceAggregate.UpdateFloor(parsedCommand.Floor);
            parkingSpaceAggregate.UpdateParkingSpaceName(parsedCommand.ParkingSpaceName);
            parkingSpaceAggregate.UpdateParkingSpaceType(parsedCommand.ParkingSpaceType);
            parkingSpaceAggregate.SetOccupied(parsedCommand.IsOccupied);

            parkingSpaceModel.UpdateBasedOnAggregate(parkingSpaceAggregate);
            databaseDbContext.ParkingSpaces.Update(parkingSpaceModel);

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as DeleteParkingSpaceCommand;

        ParkingSpaceModel? parkingSpaceModel = await databaseDbContext.ParkingSpaces
            .FirstOrDefaultAsync(ps => ps.Id == parsedCommand.Id, cancellationToken);

        if (parkingSpaceModel == null) return false;

        databaseDbContext.ParkingSpaces.Remove(parkingSpaceModel);

        return true;
    }

    public async Task<ParkingSpaceModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as GetParkingSpaceQuery;

        IQueryable<ParkingSpaceModel> databaseQuery = databaseDbContext.ParkingSpaces.Include(ps => ps.Location);

        if (parsedQuery.IncludeReservations)
            databaseQuery = databaseQuery.Include(ps => ps.Reservations);

        return await databaseQuery.FirstOrDefaultAsync(ps => ps.Id == parsedQuery.ParkingSpaceId, cancellationToken);
    }

    public async Task<IEnumerable<ParkingSpaceModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListParkingSpacesQuery;

        bool hasParkingIds = parsedQuery.ParkingSpaceIds != null && parsedQuery.ParkingSpaceIds.Any();
        IQueryable<ParkingSpaceModel> databaseQuery = databaseDbContext.ParkingSpaces.Include(ps => ps.Location);

        if (hasParkingIds)
            databaseQuery = databaseQuery.Where(ps => parsedQuery.ParkingSpaceIds!.Contains(ps.Id));

        if (parsedQuery.IncludeReservations!.Value)
            databaseQuery = databaseQuery.Include(ps => ps.Reservations);

        return await databaseQuery.ToListAsync(cancellationToken);
    }
}