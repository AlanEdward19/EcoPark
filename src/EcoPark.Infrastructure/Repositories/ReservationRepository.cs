using EcoPark.Application.Reservations.Delete;
using EcoPark.Application.Reservations.Get;
using EcoPark.Application.Reservations.Insert;
using EcoPark.Application.Reservations.List;
using EcoPark.Application.Reservations.Update;
using EcoPark.Domain.Aggregates.Location.ParkingSpace;

namespace EcoPark.Infrastructure.Repositories;

public class ReservationRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork) : IRepository<ReservationModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        ReservationModel? reservationModel = null;
        ClientModel? client;
        var requestUserInfo = command.RequestUserInfo;

        switch (command.GetType().Name)
        {
            case nameof(UpdateReservationCommand):
                var parsedUpdateCommand = command as UpdateReservationCommand;

                reservationModel = await databaseDbContext.Reservations
                    .AsNoTracking()
                    .Include(x => x.Client)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x =>
                                                   x.Id.Equals(parsedUpdateCommand.ReservationId) && x.Client.Email.Equals(requestUserInfo.Email),
                                               cancellationToken);

                break;

            case nameof(DeleteReservationCommand):
                var parsedDeleteCommand = command as DeleteReservationCommand;

                if(requestUserInfo.UserType != "Client")
                    return true;

                reservationModel = await databaseDbContext.Reservations
                    .AsNoTracking()
                    .Include(x => x.Client)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x =>
                            x.Id.Equals(parsedDeleteCommand.Id) && x.Client.Email.Equals(
                                parsedDeleteCommand.RequestUserInfo.Email),
                        cancellationToken);

                break;

            case nameof(InsertReservationCommand):
                var parsedInsertCommand = command as InsertReservationCommand;

                client =
                    await databaseDbContext.Clients
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Include(x => x.Cars)
                        .FirstOrDefaultAsync(
                            x => x.Email.Equals(requestUserInfo.Email),
                            cancellationToken);

                if (client == null || !client.Cars.Any())
                    return false;

                int i = 0;

                while (i < client.Cars.Count)
                {
                    if (client.Cars.ElementAt(i).Id.Equals(parsedInsertCommand.CarId))
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
            .FirstOrDefaultAsync(e => e.Email.Equals(requestUserInfo.Email),
                cancellationToken);

        ParkingSpaceModel? parkingSpaceModel = await databaseDbContext.ParkingSpaces
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Location)
            .FirstOrDefaultAsync(e => e.Id == parsedCommand.ParkingSpaceId, cancellationToken);

        if (clientModel == null || parkingSpaceModel == null) return false;

        ReservationModel reservationModel = new(parsedCommand.ParkingSpaceId, clientModel.Id,
            parsedCommand.CarId, parsedCommand.ReservationDate, Reservation.GenerateReservationCode(), parkingSpaceModel.Location.ReservationGraceInMinutes);

        await databaseDbContext.Reservations.AddAsync(reservationModel, cancellationToken);

        return true;
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UpdateReservationCommand;

        ReservationModel? reservationModel = await databaseDbContext.Reservations
            .AsSplitQuery()
            .Include(x => x.ParkingSpace)
            .ThenInclude(parkingSpaceModel => parkingSpaceModel.Location)
            .FirstOrDefaultAsync(r => r.Id == parsedCommand.ReservationId, cancellationToken);

        if (reservationModel != null)
        {
            Reservation reservation = new(reservationModel);

            reservation.ChangeReservationDate(parsedCommand.ReservationDate,
                reservationModel.ParkingSpace.Location.ReservationGraceInMinutes);

            reservationModel.UpdateBasedOnValueObject(reservation);

            databaseDbContext.Reservations.Update(reservationModel);

            return true;
        }

        return false;
    }

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

        if (requestUserInfo.UserType == "Client")
        {
            ClientModel? clientModel = await databaseDbContext.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Email.Equals(requestUserInfo.Email),
                    cancellationToken);

            if (clientModel == null) return null;

            reservationQuery = reservationQuery
                .Where(x => x.ClientId.Equals(clientModel.Id));
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

        if (requestUserInfo.UserType == "Client")
        {
            ClientModel? clientModel = await databaseDbContext.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Email.Equals(requestUserInfo.Email),
                    cancellationToken);

            if (clientModel == null) return Enumerable.Empty<ReservationModel>();

            reservationQuery = reservationQuery
                .Where(x => x.ClientId.Equals(clientModel.Id));
        }
        
        if (parsedQuery.IncludeParkingSpace)
            reservationQuery = reservationQuery.Include(r => r.ParkingSpace);

        if (parsedQuery.ReservationIds != null && parsedQuery.ReservationIds.Any())
            reservationQuery = reservationQuery.Where(x => parsedQuery.ReservationIds.Contains(x.Id));

        return await reservationQuery.ToListAsync(cancellationToken);
    }
}