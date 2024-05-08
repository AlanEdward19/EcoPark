using EcoPark.Application.Rewards.Insert;
using EcoPark.Application.Rewards.Update;
using EcoPark.Domain.Interfaces.Providers;

namespace EcoPark.Infrastructure.Repositories;

public class RewardRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork, IStorageProvider storageProvider) : IRepository<RewardModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        return true; //fazer validações
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
        throw new NotImplementedException();
    }

    public async Task<RewardModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RewardModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}