using EcoPark.Application.Rewards.Delete;
using EcoPark.Application.Rewards.Get;
using EcoPark.Application.Rewards.Insert;
using EcoPark.Application.Rewards.List;
using EcoPark.Application.Rewards.Update;
using EcoPark.Domain.Interfaces.Providers;

namespace EcoPark.Infrastructure.Repositories;

public class RewardRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork, IStorageProvider storageProvider) : IRepository<RewardModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        EmployeeModel? owner;
        LocationModel? location;
        RewardModel? reward;
        var requestUserInfo = command.RequestUserInfo;

        switch (command)
        {
            case InsertRewardCommand insertCommand:
                location = await databaseDbContext.Locations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id.Equals(insertCommand.LocationId.Value), cancellationToken);

                if (location == null) return false;

                owner = await databaseDbContext.Employees
                    .Include(x => x.Credentials)
                    .Include(x => x.Employees)
                    .ThenInclude(x => x.Credentials)
                    .AsNoTracking()
                    .AsSplitQuery()
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

            case UpdateRewardCommand updateCommand:
                reward = await databaseDbContext.Rewards
                    .Include(x => x.Location)
                    .FirstOrDefaultAsync(x => x.Id.Equals(updateCommand.Id), cancellationToken);

                if (reward == null) return false;

                owner = await databaseDbContext.Employees
                    .Include(x => x.Credentials)
                    .Include(x => x.Employees)
                    .ThenInclude(x => x.Credentials)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id.Equals(reward.Location.OwnerId), cancellationToken);

                if (owner == null) return false;

                if (owner.Credentials.Email.Equals(requestUserInfo.Email)) return true;

                if (owner.Employees.Select(x => x.Credentials.Email).Contains(requestUserInfo.Email))
                {
                    EmployeeModel employeeModel = (await databaseDbContext.Employees
                        .Include(x => x.Credentials)
                        .Include(x => x.GroupAccesses)
                        .FirstOrDefaultAsync(x => x.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken))!;

                    return employeeModel.GroupAccesses.Any(x => x.LocationId.Equals(reward.Location.Id));
                }

                break;

            case DeleteRewardCommand deleteCommand:
                reward = await databaseDbContext.Rewards
                    .Include(x => x.Location)
                    .FirstOrDefaultAsync(x => x.Id.Equals(deleteCommand.Id), cancellationToken);

                if (reward == null) return false;

                owner = await databaseDbContext.Employees
                    .Include(x => x.Credentials)
                    .Include(x => x.Employees)
                    .ThenInclude(x => x.Credentials)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id.Equals(reward.Location.OwnerId), cancellationToken);

                if (owner == null) return false;

                if (owner.Credentials.Email.Equals(requestUserInfo.Email)) return true;

                if (owner.Employees.Select(x => x.Credentials.Email).Contains(requestUserInfo.Email))
                {
                    EmployeeModel employeeModel = (await databaseDbContext.Employees
                        .Include(x => x.Credentials)
                        .Include(x => x.GroupAccesses)
                        .FirstOrDefaultAsync(x => x.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken))!;

                    return employeeModel.GroupAccesses.Any(x => x.LocationId.Equals(reward.Location.Id));
                }

                break;
        }

        return false;
    }

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertRewardCommand;

        Guid imageId = Guid.NewGuid();
        string format = parsedCommand.ImageFileName.Split('.').Last();
        string blobFileName = $"{imageId}.{format}";

        await storageProvider.WriteBlobAsync(parsedCommand.Image, blobFileName, "rewards");

        RewardModel reward = new(parsedCommand!.Name, parsedCommand!.Description, parsedCommand.AvailableQuantity,
            parsedCommand.RequiredPoints!.Value, parsedCommand.Url, blobFileName, parsedCommand!.IsActive.Value, parsedCommand.ExpirationDate,
            parsedCommand.LocationId!.Value);

        await databaseDbContext.Rewards.AddAsync(reward, cancellationToken);

        return true;
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UpdateRewardCommand;

        RewardModel? reward = await databaseDbContext.Rewards
            .Include(x => x.Location)
            .FirstOrDefaultAsync(e => e.Id == parsedCommand.Id, cancellationToken);

        if (reward != null)
        {
            RewardValueObject rewardValueObject = new(reward);

            rewardValueObject.UpdateName(parsedCommand.Name);
            rewardValueObject.UpdateDescription(parsedCommand.Description);
            rewardValueObject.UpdateAvailableQuantity(parsedCommand.AvailableQuantity);
            rewardValueObject.UpdateRequiredPoints(parsedCommand.RequiredPoints);
            rewardValueObject.UpdateIsActive(parsedCommand.IsActive);
            rewardValueObject.UpdateUrl(parsedCommand.Url);
            rewardValueObject.UpdateExpirationDate(parsedCommand.ExpirationDate);

            if (parsedCommand.Image != null)
            {
                string blobName = reward.Image;
                string newFileFormat = parsedCommand.ImageFileName!.Split('.').Last();
                string oldFileFormat = reward.Image.Split('.').Last();

                if (!newFileFormat.Equals(oldFileFormat))
                {
                    string oldFileName = reward.Image.Split(".").First();
                    blobName = $"{oldFileName}.{newFileFormat}";

                    rewardValueObject.UpdateImage(blobName);
                }

                await storageProvider.DeleteBlobAsync(reward.Image, "rewards");
                await storageProvider.WriteBlobAsync(parsedCommand.Image, blobName, "rewards");
            }

            reward.UpdateBasedOnValueObject(rewardValueObject);
            databaseDbContext.Rewards.Update(reward);

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as DeleteRewardCommand;

        RewardModel? reward = await databaseDbContext.Rewards
            .Include(x => x.Location)
            .FirstOrDefaultAsync(e => e.Id == parsedCommand.Id, cancellationToken);

        if (reward == null) return false;

        await storageProvider.DeleteBlobAsync(reward.Image, "rewards");
        databaseDbContext.Rewards.Remove(reward);

        return true;
    }

    public async Task<RewardModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as GetRewardQuery;

        return await databaseDbContext.Rewards
            .FirstOrDefaultAsync(e => e.Id == parsedQuery.RewardId, cancellationToken);
    }

    public async Task<IEnumerable<RewardModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListRewardsQuery;

        IQueryable<RewardModel> databaseQuery = databaseDbContext.Rewards
            .Include(x => x.Location)
            .AsQueryable()
            .AsSplitQuery()
            .AsNoTracking();

        if (parsedQuery.LocationId != null)
            databaseQuery = databaseQuery.Where(e => e.LocationId == parsedQuery.LocationId);

        if (parsedQuery.RewardIds != null && parsedQuery.RewardIds.Any())
            databaseQuery = databaseQuery.Where(e => parsedQuery.RewardIds.Contains(e.Id));

        return await databaseQuery.ToListAsync(cancellationToken);
    }
}