using EcoPark.Application.Cars.Delete;
using EcoPark.Application.Cars.Get;
using EcoPark.Application.Cars.Insert;
using EcoPark.Application.Cars.List;
using EcoPark.Application.Cars.Update;

namespace EcoPark.Infrastructure.Repositories;

public class CarRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork) : IRepository<CarModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<EOperationStatus> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        CarModel? carModel = null;
        var requestUserInfo = command.RequestUserInfo;

        switch (command)
        {
            case InsertCarCommand insertCommand:

                ClientModel? clientModel = await databaseDbContext.Clients
                    .AsNoTracking()
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                        cancellationToken);

                if (clientModel == null) return EOperationStatus.Failed;

                return EOperationStatus.Successful;

            case UpdateCarCommand updateCommand:

                carModel = await databaseDbContext.Cars
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(x => x.Client)
                    .ThenInclude(x => x.Credentials)
                    .FirstOrDefaultAsync(e => e.Id == updateCommand.CarId, cancellationToken);

                if(carModel == null) return EOperationStatus.NotFound;

                if (!carModel.Client.Credentials.Email.Equals(requestUserInfo.Email))
                    return EOperationStatus.NotAuthorized;

                break;

            case DeleteCarCommand deleteCommand:

                carModel = await databaseDbContext.Cars
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(x => x.Client)
                    .ThenInclude(x => x.Credentials)
                    .FirstOrDefaultAsync(e => e.Id == deleteCommand.Id, cancellationToken);

                if (carModel == null) return EOperationStatus.NotFound;

                if (!carModel.Client.Credentials.Email.Equals(requestUserInfo.Email))
                    return EOperationStatus.NotAuthorized;

                break;
        }

        return EOperationStatus.Failed;
    }

    public async Task AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertCarCommand;

        var requestUserInfo = command.RequestUserInfo;

        ClientModel clientModel = await databaseDbContext.Clients
            .AsNoTracking()
            .Include(x => x.Credentials)
            .FirstAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email),
                cancellationToken);

        CarModel carModel = parsedCommand!.ToModel(clientModel.Id);

        await databaseDbContext.Cars.AddAsync(carModel, cancellationToken);
    }

    public async Task UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UpdateCarCommand;

        CarModel carModel = await databaseDbContext.Cars
            .FirstAsync(e => e.Id == parsedCommand.CarId, cancellationToken);

        Car car = new(carModel);

        car.UpdateBrand(parsedCommand.Brand);
        car.UpdateColor(parsedCommand.Color);
        car.UpdateModel(parsedCommand.Model);
        car.UpdatePlate(parsedCommand.Plate);
        car.UpdateType(parsedCommand.Type);
        car.UpdateYear(parsedCommand.Year);
        car.UpdateFuelType(parsedCommand.FuelType);
        car.UpdateFuelConsumptionPerLiter(parsedCommand.FuelConsumptionPerLiter);

        carModel.UpdateBasedOnValueObject(car);

        databaseDbContext.Cars.Update(carModel);
    }

    public async Task DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as DeleteCarCommand;

        CarModel carModel = await databaseDbContext.Cars
            .FirstAsync(e => e.Id == parsedCommand.Id, cancellationToken);

        databaseDbContext.Cars.Remove(carModel);
    }

    public async Task<CarModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as GetCarQuery;

        var requestUserInfo = parsedQuery.RequestUserInfo;

        ClientModel? clientModel = await databaseDbContext.Clients
            .Include(x => x.Credentials)
            .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken);

        if (clientModel == null) return null;

        return await databaseDbContext.Cars
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(e => e.Id == parsedQuery.CarId && e.ClientId.Equals(clientModel.Id),
                cancellationToken);
    }

    public async Task<IEnumerable<CarModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListCarQuery;

        bool hasCarIds = parsedQuery.CarIds != null && parsedQuery.CarIds.Any();

        var requestUserInfo = parsedQuery.RequestUserInfo;

        ClientModel? clientModel = await databaseDbContext.Clients
            .Include(x => x.Credentials)
            .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken);

        if (clientModel == null) return Enumerable.Empty<CarModel>();

        IQueryable<CarModel> databaseQuery = databaseDbContext.Cars
            .AsNoTracking()
            .AsQueryable();

        databaseQuery = databaseQuery
            .Where(c => c.ClientId.Equals(clientModel.Id));

        if (hasCarIds)
            databaseQuery = databaseQuery
                .Where(c => parsedQuery.CarIds!.Contains(c.Id));

        return await databaseQuery.ToListAsync(cancellationToken);
    }
}