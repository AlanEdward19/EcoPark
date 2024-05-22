using EcoPark.Application.CarbonEmission.Delete;
using EcoPark.Application.CarbonEmission.Insert;
using EcoPark.Application.CarbonEmission.List;

namespace EcoPark.Infrastructure.Repositories;

public class CarbonEmissionRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork) : IRepository<CarbonEmissionModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<EOperationStatus> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        var requestUserInfo = command.RequestUserInfo;

        switch (command)
        {
            case InsertCarbonEmissionCommand insertCommand:

                ClientModel? clientModel = await databaseDbContext.Clients
                    .AsNoTracking()
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                                               cancellationToken);

                if (clientModel == null) return EOperationStatus.Failed;

                ReservationModel? reservationModel = await databaseDbContext.Reservations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        e => e.Id == insertCommand.ReservationId && e.Status == EReservationStatus.Confirmed,
                        cancellationToken);

                if (reservationModel == null) return EOperationStatus.NotFound;

                if (reservationModel.ClientId != clientModel.Id)
                    return EOperationStatus.NotAuthorized;

                return EOperationStatus.Successful;

            case DeleteCarbonEmissionCommand deleteCommand:
                EmployeeModel? employeeModel = await databaseDbContext.Employees
                    .AsNoTracking()
                    .Include(x => x.Credentials)
                    .Include(x => x.Administrator)
                    .ThenInclude(employeeModel => employeeModel.Credentials)
                    .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                        cancellationToken);

                if (employeeModel == null) return EOperationStatus.Failed;

                if (employeeModel.Administrator is {Credentials.UserType: EUserType.PlatformAdministrator})
                    return EOperationStatus.Successful;

                break;
        }

        return EOperationStatus.NotAuthorized;
    }

    public async Task AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertCarbonEmissionCommand;

        var requestUserInfo = parsedCommand!.RequestUserInfo;

        ClientModel clientModel = await databaseDbContext.Clients
            .AsNoTracking()
            .Include(x => x.Credentials)
            .FirstAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                cancellationToken);

        CarbonEmissionModel model = parsedCommand!.ToModel(clientModel.Id);

        await databaseDbContext.CarbonEmissions.AddAsync(model, cancellationToken);
    }

    public async Task UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as DeleteCarbonEmissionCommand;

        CarbonEmissionModel model = await databaseDbContext.CarbonEmissions
            .FirstAsync(e => e.Id == parsedCommand!.Id, cancellationToken);

        databaseDbContext.CarbonEmissions.Remove(model);
    }

    public async Task<CarbonEmissionModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CarbonEmissionModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListCarbonEmissionsQuery;

        bool hasCarbonEmissionsIds = parsedQuery!.CarbonEmissionsIds.Any();

        var requestUserInfo = parsedQuery!.RequestUserInfo;

        ClientModel clientModel = await databaseDbContext.Clients
            .AsNoTracking()
            .Include(x => x.Credentials)
            .FirstAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                               cancellationToken);

        IQueryable<CarbonEmissionModel> databaseQuery = databaseDbContext.CarbonEmissions
            .Include(x => x.Reservation)
            .ThenInclude(x => x.ParkingSpace)
            .AsNoTracking()
            .AsQueryable();

        databaseQuery = databaseQuery
            .Where(c => c.ClientId.Equals(clientModel.Id) && c.IsConfirmed);

        if (hasCarbonEmissionsIds)
            databaseQuery = databaseQuery
                .Where(c => parsedQuery!.CarbonEmissionsIds.Contains(c.Id));


        return await databaseQuery.ToListAsync(cancellationToken);
    }
}