using EcoPark.Application.Locations.Delete;
using EcoPark.Application.Locations.Get;
using EcoPark.Application.Locations.Insert;
using EcoPark.Application.Locations.List;
using EcoPark.Application.Locations.Update;
using EcoPark.Domain.Aggregates.Location;
using EcoPark.Domain.Commons.Enums;

namespace EcoPark.Infrastructure.Repositories;

public class LocationRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork) : IAggregateRepository<LocationModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        var requestUserInfo = command.RequestUserInfo;
        LocationModel? locationModel = null;

        var databaseQuery = databaseDbContext.Locations
            .AsQueryable().AsNoTracking();

        switch (command)
        {
            case InsertLocationCommand:
                return requestUserInfo.UserType == EUserType.Administrator;

            case UpdateLocationCommand updateCommand:
                databaseQuery = databaseQuery
                    .Where(x => x.Id.Equals(updateCommand.LocationId));

                var employeeModel = await databaseDbContext.Employees
                    .AsNoTracking()
                    .Include(x => x.GroupAccesses)
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken);

                if (employeeModel == null) return false;

                if (employeeModel.Credentials.UserType == EUserType.Administrator)
                    locationModel = await databaseQuery
                        .FirstOrDefaultAsync(l => l.OwnerId.Equals(employeeModel.Id),
                            cancellationToken);

                else
                {
                    locationModel = await databaseQuery
                        .FirstOrDefaultAsync(cancellationToken);

                    return employeeModel.GroupAccesses.Any(x => x.LocationId.Equals(locationModel.Id));
                }

                break;

            case DeleteLocationCommand deleteCommand:
                var administrator = await databaseDbContext.Employees
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(x =>
                        x.Credentials.Email.Equals(requestUserInfo.Email) &&
                        x.Credentials.UserType == EUserType.Administrator, cancellationToken);

                if (administrator == null) return false;

                locationModel = await databaseDbContext.Locations
                    .FirstOrDefaultAsync(x => x.Id.Equals(deleteCommand.Id) &&
                                              x.OwnerId.Equals(administrator.Id),
                        cancellationToken);

                break;
        }

        return locationModel != null;
    }

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertLocationCommand;

        var requestUserInfo = command.RequestUserInfo;

        EmployeeModel? employeeModel = await databaseDbContext.Employees
            .Include(x => x.Credentials)
            .FirstOrDefaultAsync(
                e => e.Credentials.Email.Equals(requestUserInfo.Email) &&
                     e.Credentials.UserType == EUserType.Administrator, cancellationToken);

        if (employeeModel == null) return false;

        LocationModel locationModel = new(employeeModel.Id, parsedCommand.Name, parsedCommand.Address,
            parsedCommand.ReservationGraceInMinutes!.Value, parsedCommand.CancellationFeeRate!.Value,
            parsedCommand.ReservationFeeRate!.Value, parsedCommand.HourlyParkingRate!.Value);

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
            locationAggregate.UpdateReservationGraceInMinutes(parsedCommand.ReservationGraceInMinutes);
            locationAggregate.UpdateCancellationFeeRate(parsedCommand.CancellationFeeRate);
            locationAggregate.UpdateReservationFeeRate(parsedCommand.ReservationFeeRate);
            locationAggregate.UpdateHourlyParkingRate(parsedCommand.HourlyParkingRate);

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

        var requestUserInfo = query.RequestUserInfo;

        EmployeeModel? employeeModel;

        IQueryable<LocationModel> databaseQuery = databaseDbContext.Locations
            .AsNoTracking();

        if (requestUserInfo.UserType == EUserType.Administrator)
        {
            employeeModel = await databaseDbContext.Employees
                .Include(x => x.Credentials)
                .FirstOrDefaultAsync(
                    e => e.Credentials.Email.Equals(requestUserInfo.Email) &&
                         e.Credentials.UserType == EUserType.Administrator, cancellationToken);

            if (employeeModel == null) return null;

            databaseQuery = databaseQuery
                .Where(x => x.OwnerId.Equals(employeeModel.Id));
        }
        else if(requestUserInfo.UserType != EUserType.PlataformAdministrator)
        {
            employeeModel = await databaseDbContext.Employees
                .Include(x => x.Credentials)
                .Include(x => x.GroupAccesses)
                .FirstOrDefaultAsync(
                    e => e.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken);

            if (employeeModel == null) return null;

            databaseQuery = databaseQuery
                .Where(x => employeeModel.GroupAccesses
                    .Any(y => y.LocationId.Equals(x.Id)));
        }

        if (parsedQuery.IncludeParkingSpaces!.Value)
            databaseQuery = databaseQuery.Include(l => l.ParkingSpaces);

        LocationModel? locationModel =
            await databaseQuery.FirstOrDefaultAsync(l => l.Id == parsedQuery.LocationId, cancellationToken);

        return locationModel;
    }

    public async Task<IEnumerable<LocationModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListLocationQuery;

        var requestUserInfo = query.RequestUserInfo;

        EmployeeModel? employeeModel;

        IQueryable<LocationModel> databaseQuery = databaseDbContext.Locations
            .AsNoTracking();

        if (requestUserInfo.UserType == EUserType.Administrator)
        {
            employeeModel = await databaseDbContext.Employees
                .Include(x => x.Credentials)
                .FirstOrDefaultAsync(
                    e => e.Credentials.Email.Equals(requestUserInfo.Email) &&
                         e.Credentials.UserType == EUserType.Administrator, cancellationToken);

            if (employeeModel == null) return Enumerable.Empty<LocationModel>();

            databaseQuery = databaseQuery
                .Where(x => x.OwnerId.Equals(employeeModel.Id));
        }
        else if(requestUserInfo.UserType != EUserType.PlataformAdministrator)
        {
            employeeModel = await databaseDbContext.Employees
                .Include(x => x.Credentials)
                .Include(x => x.GroupAccesses)
                .FirstOrDefaultAsync(
                    e => e.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken);

            if (employeeModel == null) return Enumerable.Empty<LocationModel>();

            databaseQuery = databaseQuery
                .Where(x => employeeModel.GroupAccesses
                    .Any(y => y.LocationId.Equals(x.Id)));
        }

        bool hasLocationIds = parsedQuery.LocationIds != null && parsedQuery.LocationIds.Any();

        if (hasLocationIds)
            databaseQuery = databaseQuery.Where(l => parsedQuery.LocationIds!.Contains(l.Id));

        if (parsedQuery.IncludeParkingSpaces!.Value)
            databaseQuery = databaseQuery.Include(l => l.ParkingSpaces);

        return await databaseQuery.ToListAsync(cancellationToken);
    }
}