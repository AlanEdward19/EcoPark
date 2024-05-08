using EcoPark.Application.Reservations.Delete;
using EcoPark.Application.Reservations.Get;
using EcoPark.Application.Reservations.Insert;
using EcoPark.Application.Reservations.List;
using EcoPark.Application.Reservations.Update;
using EcoPark.Application.Reservations.Update.Status;
using EcoPark.Domain.Aggregates.Location.ParkingSpace;
using Microsoft.Extensions.Logging;

namespace EcoPark.Infrastructure.Repositories;

public class ReservationRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork, ILogger<ReservationRepository> logger) : IRepository<ReservationModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        ReservationModel? reservationModel = null;
        ClientModel? client;
        var requestUserInfo = command.RequestUserInfo;

        switch (command)
        {
            case UpdateReservationStatusCommand:
                return requestUserInfo.UserType == EUserType.System;

            case UpdateReservationCommand updateCommand:

                reservationModel = await databaseDbContext.Reservations
                    .AsNoTracking()
                    .Include(x => x.Client)
                    .ThenInclude(x => x.Credentials)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x =>
                            x.Id.Equals(updateCommand.ReservationId) &&
                            x.Client.Credentials.Email.Equals(requestUserInfo.Email),
                        cancellationToken);

                break;

            case DeleteReservationCommand deleteCommand:

                if (requestUserInfo.UserType == EUserType.PlataformAdministrator)
                    return true;

                if (requestUserInfo.UserType is EUserType.Administrator or EUserType.Employee)
                {
                    reservationModel = await databaseDbContext.Reservations
                        .AsNoTracking()
                        .Include(x => x.Client)
                        .ThenInclude(x => x.Credentials)
                        .AsSplitQuery()
                        .FirstOrDefaultAsync(x =>
                                x.Id.Equals(deleteCommand.Id), cancellationToken);

                    if (reservationModel == null) return false;

                    var locationModel = await databaseDbContext.Locations
                        .Include(x => x.ParkingSpaces)
                        .FirstOrDefaultAsync(x =>
                            x.ParkingSpaces.Any(y => y.Id.Equals(reservationModel.ParkingSpaceId)), cancellationToken);

                    if (locationModel == null) return false;

                    var owner = await databaseDbContext.Employees
                        .Include(x => x.Credentials)
                        .Include(x => x.Employees)
                        .ThenInclude(x => x.Credentials)
                        .FirstOrDefaultAsync(x => x.Id.Equals(locationModel.OwnerId), cancellationToken);

                    if (owner == null) return false;

                    if (owner.Credentials.Email.Equals(requestUserInfo.Email)) return true;

                    if (owner.Employees.Select(x => x.Credentials.Email).Contains(requestUserInfo.Email))
                    {
                        EmployeeModel employeeModel = (await databaseDbContext.Employees
                            .Include(x => x.Credentials)
                            .Include(x => x.GroupAccesses)
                            .FirstOrDefaultAsync(x => x.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken))!;

                        return employeeModel.GroupAccesses.Any(x => x.LocationId.Equals(locationModel.Id));
                    }
                }

                reservationModel = await databaseDbContext.Reservations
                    .AsNoTracking()
                    .Include(x => x.Client)
                    .ThenInclude(x => x.Credentials)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x =>
                            x.Id.Equals(deleteCommand.Id) && x.Client.Credentials.Email.Equals(
                                deleteCommand.RequestUserInfo.Email),
                        cancellationToken);

                break;

            case InsertReservationCommand insertCommand:

                ReservationModel? reservation = await databaseDbContext.Reservations
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(x => x.ParkingSpace)
                    .Where(x => !x.ParkingSpace.IsOccupied &&
                                (x.Status != EReservationStatus.Completed &&
                                 x.Status != EReservationStatus.Cancelled &&
                                 x.Status != EReservationStatus.Expired) &&
                                x.ParkingSpaceId.Equals(insertCommand!.ParkingSpaceId) &&
                                x.ReservationDate.Equals(insertCommand!.ReservationDate) ||
                                x.ExpirationDate >= insertCommand!.ReservationDate)
                    .FirstOrDefaultAsync(cancellationToken);

                if (reservation != null) return false;

                client =
                    await databaseDbContext.Clients
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Include(x => x.Credentials)
                        .Include(x => x.Cars)
                        .FirstOrDefaultAsync(
                            x => x.Credentials.Email.Equals(requestUserInfo.Email),
                            cancellationToken);

                if (client == null || !client.Cars!.Any())
                    return false;

                int i = 0;

                while (i < client.Cars.Count)
                {
                    if (client.Cars.ElementAt(i).Id.Equals(insertCommand!.CarId))
                        return true;

                    i++;
                }

                break;
        }

        return reservationModel != null;
    }

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertReservationCommand;

        var requestUserInfo = command.RequestUserInfo;

        ClientModel? clientModel = await databaseDbContext.Clients
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Credentials)
            .Include(x => x.Cars)
            .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                cancellationToken);

        ParkingSpaceModel? parkingSpaceModel = await databaseDbContext.ParkingSpaces
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Location)
            .FirstOrDefaultAsync(e => e.Id == parsedCommand.ParkingSpaceId, cancellationToken);

        if (clientModel == null || parkingSpaceModel == null) return false;

        ReservationModel reservationModel;

        EReservationStatus reservationStatus;

        if (parkingSpaceModel.Location.ReservationFeeRate > 0)
        {
            logger.LogInformation("Chamando gateway de pagamento [Mock]");
            reservationStatus = EReservationStatus.Created;
        }
        else
            reservationStatus = EReservationStatus.Confirmed;

        ReservationModel? lastReservation = await databaseDbContext.Reservations
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Client)
            .ThenInclude(x => x.Credentials)
            .OrderByDescending(x => x.ReservationDate)
            .FirstOrDefaultAsync(x =>
                                   x.Client.Credentials.Email.Equals(requestUserInfo.Email) && x.Status == EReservationStatus.Completed,
                               cancellationToken);

        string reservationCode = Reservation.GenerateReservationCode();

        bool reservationCodeAlreadyExists = await databaseDbContext.Reservations
            .AsNoTracking()
            .AnyAsync(
                x => x.ReservationCode.Equals(reservationCode) && x.Status != EReservationStatus.Completed &&
                     x.Status != EReservationStatus.Cancelled &&
                     x.Status != EReservationStatus.Expired, cancellationToken);

        while (reservationCodeAlreadyExists)
        {
            reservationCode = Reservation.GenerateReservationCode();
            reservationCodeAlreadyExists = await databaseDbContext.Reservations
                .AsNoTracking()
                .AnyAsync(
                    x => x.ReservationCode.Equals(reservationCode) && x.Status != EReservationStatus.Completed &&
                         x.Status != EReservationStatus.Cancelled &&
                         x.Status != EReservationStatus.Expired, cancellationToken);
        }

        reservationModel = new(parsedCommand!.ParkingSpaceId!.Value, clientModel.Id,
            parsedCommand.CarId!.Value, parsedCommand.ReservationDate!.Value, reservationCode,
            parkingSpaceModel.Location.ReservationGraceInMinutes, reservationStatus,
            Reservation.CalculatePunctuation(parsedCommand.ReservationDate.Value, lastReservation?.ReservationDate,
                clientModel.Cars.FirstOrDefault(x => x.Id.Equals(parsedCommand.CarId))!.Type,
                parkingSpaceModel.Location.ReservationFeeRate));

        await databaseDbContext.Reservations.AddAsync(reservationModel, cancellationToken);

        return true;
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken) => command.GetType().Name switch
    {
        nameof(UpdateReservationCommand) => await UpdateReservationAsync(command, cancellationToken),
        nameof(UpdateReservationStatusCommand) => await UpdateReservationStatusAsync(command, cancellationToken)
    };

    public async Task<bool> DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as DeleteReservationCommand;

        ReservationModel? reservationModel = await databaseDbContext.Reservations
            .FirstOrDefaultAsync(r => r.Id == parsedCommand.Id, cancellationToken);

        if (reservationModel == null) return false;

        databaseDbContext.Reservations.Remove(reservationModel);

        return true;
    }

    public async Task<ReservationModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as GetReservationQuery;

        var requestUserInfo = query.RequestUserInfo;

        IQueryable<ReservationModel> reservationQuery = databaseDbContext.Reservations
            .AsNoTracking()
            .AsQueryable();

        if (requestUserInfo.UserType == EUserType.Client)
        {
            ClientModel? clientModel = await databaseDbContext.Clients
                .AsNoTracking()
                .Include(x => x.Credentials)
                .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                    cancellationToken);

            if (clientModel == null) return null;

            reservationQuery = reservationQuery
                .Where(x => x.ClientId.Equals(clientModel.Id));
        }

        else if (requestUserInfo.UserType is EUserType.Administrator)
        {
            EmployeeModel? employeeModel = await databaseDbContext.Employees
                .AsNoTracking()
                .Include(x => x.Credentials)
                .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                    cancellationToken);

            if (employeeModel == null) return null;

            reservationQuery = reservationQuery.Include(x => x.ParkingSpace)
                .ThenInclude(x => x.Location);

            reservationQuery = reservationQuery
                .Where(x => x.ParkingSpace.Location.OwnerId.Equals(employeeModel.Id));
        }

        else if (requestUserInfo.UserType is EUserType.Employee)
        {
            EmployeeModel? employeeModel = await databaseDbContext.Employees
                .AsNoTracking()
                .Include(x => x.Credentials)
                .Include(x => x.GroupAccesses)
                .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                    cancellationToken);

            if (employeeModel == null) return null;

            reservationQuery = reservationQuery
                .Where(x => employeeModel.GroupAccesses
                    .Any(y => y.LocationId.Equals(x.ParkingSpace.LocationId)));
        }

        if (parsedQuery.IncludeParkingSpace)
            reservationQuery = reservationQuery.Include(r => r.ParkingSpace);

        return await reservationQuery.FirstOrDefaultAsync(r => r.Id == parsedQuery.ReservationId, cancellationToken);
    }

    public async Task<IEnumerable<ReservationModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListReservationQuery;

        var requestUserInfo = query.RequestUserInfo;

        IQueryable<ReservationModel> reservationQuery =
            databaseDbContext.Reservations
                .AsNoTracking()
                .AsQueryable();

        if (requestUserInfo.UserType == EUserType.Client)
        {
            ClientModel? clientModel = await databaseDbContext.Clients
                .AsNoTracking()
                .Include(x => x.Credentials)
                .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                    cancellationToken);

            if (clientModel == null) return Enumerable.Empty<ReservationModel>();

            reservationQuery = reservationQuery
                .Where(x => x.ClientId.Equals(clientModel.Id));
        }

        else if (requestUserInfo.UserType is EUserType.Administrator)
        {
            EmployeeModel? employeeModel = await databaseDbContext.Employees
                .AsNoTracking()
                .Include(x => x.Credentials)
                .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                                       cancellationToken);

            if (employeeModel == null) return Enumerable.Empty<ReservationModel>();

            reservationQuery = reservationQuery.Include(x => x.ParkingSpace)
                .ThenInclude(x => x.Location);

            reservationQuery = reservationQuery
                .Where(x => x.ParkingSpace.Location.OwnerId.Equals(employeeModel.Id));
        }

        else if (requestUserInfo.UserType is EUserType.Employee)
        {
            EmployeeModel? employeeModel = await databaseDbContext.Employees
                .AsNoTracking()
                .Include(x => x.Credentials)
                .Include(x => x.GroupAccesses)
                .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                                       cancellationToken);

            if (employeeModel == null) return Enumerable.Empty<ReservationModel>();

            reservationQuery = reservationQuery
                .Where(x => employeeModel.GroupAccesses
                                   .Any(y => y.LocationId.Equals(x.ParkingSpace.LocationId)));
        }

        if (parsedQuery.IncludeParkingSpace)
            reservationQuery = reservationQuery.Include(r => r.ParkingSpace);

        if (parsedQuery.ReservationIds != null && parsedQuery.ReservationIds.Any())
            reservationQuery = reservationQuery.Where(x => parsedQuery.ReservationIds.Contains(x.Id));

        return await reservationQuery.ToListAsync(cancellationToken);
    }

    private async Task<bool> UpdateReservationStatusAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UpdateReservationStatusCommand;
        ReservationModel? reservationModel = null;

        if (parsedCommand!.ReservationId != null)
            reservationModel = await databaseDbContext.Reservations
                .FirstOrDefaultAsync(r => r.Id == parsedCommand.ReservationId, cancellationToken);

        else if (!string.IsNullOrWhiteSpace(parsedCommand!.ReservationCode))
            reservationModel = await databaseDbContext.Reservations
                .FirstOrDefaultAsync(
                    r => r.ReservationCode.Equals(parsedCommand.ReservationCode) &&
                         r.Status == EReservationStatus.Confirmed, cancellationToken);

        if (reservationModel != null)
        {
            Reservation reservation = new(reservationModel);

            reservation.ChangeStatus(parsedCommand.Status);

            reservationModel.UpdateBasedOnValueObject(reservation);

            databaseDbContext.Reservations.Update(reservationModel);

            return true;
        }

        return false;
    }

    private async Task<bool> UpdateReservationAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UpdateReservationCommand;

        ReservationModel? reservationModel = await databaseDbContext.Reservations
            .AsSplitQuery()
            .Include(x => x.ParkingSpace)
            .ThenInclude(parkingSpaceModel => parkingSpaceModel.Location)
            .FirstOrDefaultAsync(r => r.Id == parsedCommand!.ReservationId, cancellationToken);

        if (reservationModel != null)
        {
            Reservation reservation = new(reservationModel);

            reservation.ChangeReservationDate(parsedCommand!.ReservationDate!.Value,
                reservationModel.ParkingSpace.Location.ReservationGraceInMinutes);

            reservationModel.UpdateBasedOnValueObject(reservation);

            databaseDbContext.Reservations.Update(reservationModel);

            return true;
        }

        return false;
    }
}