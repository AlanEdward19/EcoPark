using EcoPark.Application.ParkingSpaces.Delete;
using EcoPark.Application.ParkingSpaces.Get;
using EcoPark.Application.ParkingSpaces.Insert;
using EcoPark.Application.ParkingSpaces.List;
using EcoPark.Application.ParkingSpaces.Update;
using EcoPark.Application.ParkingSpaces.Update.Status;
using EcoPark.Application.Reservations.Update.Status;
using EcoPark.Domain.Aggregates.Location.ParkingSpace;
using EcoPark.Domain.Commons.Enums;

namespace EcoPark.Infrastructure.Repositories;

public class ParkingSpaceRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork, IRepository<ReservationModel> reservationRepository) : IAggregateRepository<ParkingSpaceModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        EmployeeModel? owner;
        ParkingSpaceModel? parkingSpace;
        var requestUserInfo = command.RequestUserInfo;

        switch (command)
        {
            case UpdateParkingSpaceCommand updateCommand:
                parkingSpace = await databaseDbContext.ParkingSpaces
                    .AsNoTracking()
                    .Include(x => x.Location)
                    .ThenInclude(x => x.Owner)
                    .ThenInclude(x => x.Credentials)
                    .FirstOrDefaultAsync(x => x.Id.Equals(updateCommand.ParkingSpaceId), cancellationToken);

                owner = await databaseDbContext.Employees
                    .Include(x => x.Employees)
                    .ThenInclude(x => x.Credentials)
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(x => x.Id.Equals(parkingSpace.Location.OwnerId), cancellationToken);

                if (owner == null) return false;

                if (owner.Credentials.Email.Equals(requestUserInfo.Email)) return true;

                if (owner.Employees.Select(x => x.Credentials.Email).Contains(requestUserInfo.Email))
                {
                    EmployeeModel employeeModel = (await databaseDbContext.Employees
                        .Include(x => x.Credentials)
                        .Include(x => x.GroupAccesses)
                        .FirstOrDefaultAsync(x => x.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken))!;

                    return employeeModel.GroupAccesses.Any(x => x.LocationId.Equals(parkingSpace.LocationId));
                }

                break;

            case UpdateParkingSpaceStatusCommand:
                return requestUserInfo.UserType == EUserType.System;

            case DeleteParkingSpaceCommand deleteCommand:
                parkingSpace = await databaseDbContext.ParkingSpaces
                    .AsNoTracking()
                    .Include(x => x.Location)
                    .ThenInclude(x => x.Owner)
                    .ThenInclude(x => x.Credentials)
                    .FirstOrDefaultAsync(x => x.Id.Equals(deleteCommand.Id), cancellationToken);

                owner = await databaseDbContext.Employees
                    .Include(x => x.Employees)
                    .ThenInclude(x => x.Credentials)
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(x => x.Id.Equals(parkingSpace.Location.OwnerId), cancellationToken);

                if (owner == null) return false;

                if (owner.Credentials.Email.Equals(requestUserInfo.Email)) return true;

                if (owner.Employees.Select(x => x.Credentials.Email).Contains(requestUserInfo.Email))
                {
                    EmployeeModel employeeModel = (await databaseDbContext.Employees
                        .Include(x => x.Credentials)
                        .Include(x => x.GroupAccesses)
                        .FirstOrDefaultAsync(x => x.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken))!;

                    return employeeModel.GroupAccesses.Any(x => x.LocationId.Equals(parkingSpace.LocationId));
                }

                break;

            case InsertParkingSpaceCommand insertCommand:
                var location = await databaseDbContext.Locations
                    .AsNoTracking()
                    .Include(x => x.Owner)
                    .FirstOrDefaultAsync(x => x.Id.Equals(insertCommand.LocationId), cancellationToken);

                if (location == null) return false;

                owner = await databaseDbContext.Employees
                    .Include(x => x.Employees)
                    .ThenInclude(x => x.Credentials)
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(x => x.Id.Equals(location.OwnerId), cancellationToken);

                if (owner == null) return false;

                if (owner.Credentials.Email.Equals(requestUserInfo.Email)) return true;

                if (owner.Employees.Select(x => x.Credentials.Email).Contains(requestUserInfo.Email))
                {
                    EmployeeModel employeeModel = (await databaseDbContext.Employees
                        .Include(x => x.Credentials)
                        .Include(x => x.GroupAccesses)
                        .FirstOrDefaultAsync(x => x.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken))!;

                    return employeeModel.GroupAccesses.Any(x => x.LocationId.Equals(location.Id));
                }

                break;
        }

        return false;
    }

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertParkingSpaceCommand;

        ParkingSpaceModel parkingSpaceModel = new(parsedCommand.LocationId!.Value, parsedCommand.Floor!.Value, parsedCommand.ParkingSpaceName!,
            parsedCommand.IsOccupied!.Value, parsedCommand.Type!.Value);

        await databaseDbContext.ParkingSpaces.AddAsync(parkingSpaceModel, cancellationToken);

        return true;
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken) => command switch
    {
        UpdateParkingSpaceCommand updateParkingSpaceCommand => await UpdateParkingSpaceAsync(updateParkingSpaceCommand, cancellationToken),
        UpdateParkingSpaceStatusCommand updateParkingSpaceStatusCommand => await UpdateParkingSpaceStatusAsync(updateParkingSpaceStatusCommand, cancellationToken),
    };

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

        var requestUserInfo = query.RequestUserInfo;

        EmployeeModel? employeeModel;

        bool hasParkingIds = parsedQuery.ParkingSpaceIds != null && parsedQuery.ParkingSpaceIds.Any();
        IQueryable<ParkingSpaceModel> databaseQuery = databaseDbContext.ParkingSpaces.Include(ps => ps.Location);


        if (requestUserInfo.UserType == EUserType.Administrator)
        {
            employeeModel = await databaseDbContext.Employees
                .Include(x => x.Credentials)
                .FirstOrDefaultAsync(
                    e => e.Credentials.Email.Equals(requestUserInfo.Email) &&
                         e.Credentials.UserType == EUserType.Administrator, cancellationToken);

            if (employeeModel == null) return Enumerable.Empty<ParkingSpaceModel>();

            databaseQuery = databaseQuery
                .Where(x => x.Location.OwnerId.Equals(employeeModel.Id));
        }
        else if (requestUserInfo.UserType != EUserType.PlataformAdministrator)
        {
            employeeModel = await databaseDbContext.Employees
                .Include(x => x.Credentials)
                .Include(x => x.GroupAccesses)
                .FirstOrDefaultAsync(
                    e => e.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken);

            if (employeeModel == null) return Enumerable.Empty<ParkingSpaceModel>();

            databaseQuery = databaseQuery
                .Where(x => employeeModel.GroupAccesses
                    .Any(y => y.LocationId.Equals(x.Id)));
        }

        if (hasParkingIds)
            databaseQuery = databaseQuery.Where(ps => parsedQuery.ParkingSpaceIds!.Contains(ps.Id));

        if (parsedQuery.IncludeReservations!.Value)
            databaseQuery = databaseQuery.Include(ps => ps.Reservations);

        return await databaseQuery.ToListAsync(cancellationToken);
    }

    private async Task<bool> UpdateParkingSpaceAsync(UpdateParkingSpaceCommand command,
        CancellationToken cancellationToken)
    {
        ParkingSpaceModel? parkingSpaceModel = await databaseDbContext.ParkingSpaces
            .FirstOrDefaultAsync(p => p.Id == command.ParkingSpaceId, cancellationToken);

        if (parkingSpaceModel != null)
        {
            ParkingSpaceAggregate parkingSpaceAggregate = new(parkingSpaceModel);

            parkingSpaceAggregate.UpdateFloor(command.Floor);
            parkingSpaceAggregate.UpdateParkingSpaceName(command.ParkingSpaceName);
            parkingSpaceAggregate.UpdateParkingSpaceType(command.ParkingSpaceType);
            parkingSpaceAggregate.SetOccupied(command.IsOccupied);

            parkingSpaceModel.UpdateBasedOnAggregate(parkingSpaceAggregate);
            databaseDbContext.ParkingSpaces.Update(parkingSpaceModel);

            return true;
        }

        return false;
    }

    private async Task<bool> UpdateParkingSpaceStatusAsync(UpdateParkingSpaceStatusCommand command,
        CancellationToken cancellationToken)
    {
        ReservationModel? reservationModel = await databaseDbContext.Reservations.Include(x => x.ParkingSpace)
            .FirstOrDefaultAsync(
                x => x.ParkingSpaceId == command.Id && x.Status == EReservationStatus.Arrived &&
                     x.ParkingSpace.IsOccupied, cancellationToken);

        if (reservationModel == null)
        {
            ParkingSpaceModel? parkingSpaceModel = await databaseDbContext.ParkingSpaces
                .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            if (parkingSpaceModel == null) return false;

            if (parkingSpaceModel.IsOccupied == command.Status)
                throw new Exception(command.Status
                    ? "ParkingSpace is already occupied"
                    : "ParkingSpace is already not occupied");

            ParkingSpaceAggregate parkingSpaceAggregate = new(parkingSpaceModel);

            parkingSpaceAggregate.SetOccupied(command.Status);

            parkingSpaceModel.UpdateBasedOnAggregate(parkingSpaceAggregate);

            databaseDbContext.ParkingSpaces.Update(parkingSpaceModel);

            return true;
        }


        if (reservationModel.ParkingSpace != null)
        {
            if (reservationModel.ParkingSpace.IsOccupied == command.Status)
                throw new Exception(command.Status
                    ? "ParkingSpace is already occupied"
                    : "ParkingSpace is already not occupied");

            UpdateReservationStatusCommand reservationStatusCommand = new();
            reservationStatusCommand.SetReservationId(reservationModel.Id);
            reservationStatusCommand.SetReservationStatus(EReservationStatus.Completed);

            ParkingSpaceAggregate parkingSpaceAggregate = new(reservationModel.ParkingSpace);
            parkingSpaceAggregate.SetOccupied(command.Status);
            reservationModel.ParkingSpace.UpdateBasedOnAggregate(parkingSpaceAggregate);


            databaseDbContext.ParkingSpaces.Update(reservationModel.ParkingSpace);

            await reservationRepository.UpdateAsync(reservationStatusCommand, cancellationToken);

            return true;
        }

        return false;
    }
}