using EcoPark.Application.Cars.Delete;
using EcoPark.Application.Cars.Get;
using EcoPark.Application.Cars.Insert;
using EcoPark.Application.Cars.List;
using EcoPark.Application.Cars.Update;
using EcoPark.Domain.Aggregates.Client;

namespace EcoPark.Infrastructure.Repositories;

public class CarRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork) : IRepository<CarModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        CarModel? carModel = null;
        var requestUserInfo = command.RequestUserInfo;

        switch (command.GetType().Name)
        {
            case nameof(UpdateCarCommand):
                var parsedUpdateCommand = command as UpdateCarCommand;

                carModel = await databaseDbContext.Cars
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(x => x.Client)
                    .FirstOrDefaultAsync(
                        e => e.Id == parsedUpdateCommand.CarId && e.Client.Email.Equals(requestUserInfo.Email),
                        cancellationToken);

                break;

            case nameof(DeleteCarCommand):
                var parsedDeleteCommand = command as DeleteCarCommand;

                carModel = await databaseDbContext.Cars
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(x => x.Client)
                    .FirstOrDefaultAsync(
                        e => e.Id == parsedDeleteCommand.Id && e.Client.Email.Equals(requestUserInfo.Email),
                        cancellationToken);

                break;
        }

        return carModel != null;
    }

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertCarCommand;

        var requestUserInfo = command.RequestUserInfo;

        ClientModel? clientModel = await databaseDbContext.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Email.Equals(requestUserInfo.Email),
                cancellationToken);

        if (clientModel == null) return false;

        CarModel carModel = parsedCommand.ToModel(clientModel.Id);

        await databaseDbContext.Cars.AddAsync(carModel, cancellationToken);

        return true;
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UpdateCarCommand;

        CarModel? carModel = await databaseDbContext.Cars
            .FirstOrDefaultAsync(e => e.Id == parsedCommand.CarId, cancellationToken);

        if (carModel != null)
        {
            Car car = new(carModel);

            car.UpdateBrand(parsedCommand.Brand);
            car.UpdateColor(parsedCommand.Color);
            car.UpdateModel(parsedCommand.Model);
            car.UpdatePlate(parsedCommand.Plate);
            car.UpdateType(parsedCommand.Type);
            car.UpdateYear(parsedCommand.Year);

            carModel.UpdateBasedOnValueObject(car);

            databaseDbContext.Cars.Update(carModel);

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as DeleteCarCommand;

        CarModel? carModel = await databaseDbContext.Cars
            .FirstOrDefaultAsync(e => e.Id == parsedCommand.Id, cancellationToken);

        if (carModel == null) return false;

        databaseDbContext.Cars.Remove(carModel);
        return true;
    }

    public async Task<CarModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as GetCarQuery;

        var requestUserInfo = parsedQuery.RequestUserInfo;

        ClientModel? clientModel = await databaseDbContext.Clients
            .FirstOrDefaultAsync(e => e.Email.Equals(requestUserInfo.Email), cancellationToken);

        if (clientModel == null) return null;

        return await databaseDbContext.Cars
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(e => e.Id == parsedQuery.CarId && e.ClientId.Equals(clientModel.Id), cancellationToken);
    }

    public async Task<IEnumerable<CarModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListCarQuery;

        bool hasCarIds = parsedQuery.CarIds != null && parsedQuery.CarIds.Any();

        var requestUserInfo = parsedQuery.RequestUserInfo;

        ClientModel? clientModel = await databaseDbContext.Clients
            .FirstOrDefaultAsync(e => e.Email.Equals(requestUserInfo.Email), cancellationToken);

        if (clientModel == null) return Enumerable.Empty<CarModel>();

        IQueryable<CarModel> databaseQuery = databaseDbContext.Cars.AsNoTracking().AsQueryable();

        databaseQuery = databaseQuery
            .Where(c => c.ClientId.Equals(clientModel.Id));

        if (hasCarIds)
            databaseQuery = databaseQuery
                .Where(c => parsedQuery.CarIds!.Contains(c.Id));

        return await databaseQuery.ToListAsync(cancellationToken);
    }
}