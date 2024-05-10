using EcoPark.Application.Rewards.Insert.RedeemReward;
using EcoPark.Application.Rewards.List.ListUserRewards;
using EcoPark.Application.Rewards.Update.UseReward;

namespace EcoPark.Infrastructure.Repositories;

public class ClientClaimedRewardRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork) : IRepository<ClientClaimedRewardModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        EmployeeModel? owner;
        LocationModel? location;
        RewardModel? reward;
        ClientModel? client;
        var requestUserInfo = command.RequestUserInfo;

        switch (command)
        {
            case RedeemRewardCommand redeemCommand:
                reward = await databaseDbContext.Rewards
                    .FirstOrDefaultAsync(
                        x => x.Id.Equals(redeemCommand.RewardId) && x.IsActive &&
                             (x.AvailableQuantity == null || x.AvailableQuantity >= 1) &&
                             (x.ExpirationDate == null || x.ExpirationDate.Value >= DateTime.Today), cancellationToken);

                if (reward == null) return false;

                client = await databaseDbContext.Clients
                    .Include(x => x.Credentials)
                    .Include(x => x.Punctuations)
                    .FirstOrDefaultAsync(x => x.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken);

                if (client == null) return false;

                return client.Punctuations.Any(x =>
                    x.LocationId.Equals(reward.LocationId) && x.Punctuation >= reward.RequiredPoints);

            case UseRewardCommand useCommand:
                ClientClaimedRewardModel? rewardClaimed = await databaseDbContext.ClientClaimedRewards
                    .FirstOrDefaultAsync(x => x.Id.Equals(useCommand.RewardId), cancellationToken);

                if (rewardClaimed == null) return false;

                client = await databaseDbContext.Clients
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(x => x.Credentials.Email.Equals(requestUserInfo.Email), cancellationToken);

                if (client == null) return false;

                return rewardClaimed.ClientId.Equals(client!.Id);
        }

        return false;
    }

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UseRewardCommand;

        string email = command.RequestUserInfo.Email;

        RewardModel? reward = await databaseDbContext.Rewards
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(parsedCommand.RewardId), cancellationToken);

        ClientModel? client = await databaseDbContext.Clients
            .AsSplitQuery()
            .Include(x => x.Credentials)
            .Include(x => x.Punctuations)
            .FirstOrDefaultAsync(x => x.Credentials.Email.Equals(email), cancellationToken);

        ClientClaimedRewardModel claimedReward = new(client!.Id, reward!.Id, false);

        client.Punctuations.First(x => x.LocationId.Equals(reward.LocationId)).Punctuation -= reward.RequiredPoints;

        databaseDbContext.Clients.Update(client);
        await databaseDbContext.ClientClaimedRewards.AddAsync(claimedReward, cancellationToken);

        return true;
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UseRewardCommand;

        string email = command.RequestUserInfo.Email;

        ClientModel? client = await databaseDbContext.Clients
            .AsSplitQuery()
            .Include(x => x.ClaimedRewards)
            .Include(x => x.Credentials)
            .FirstOrDefaultAsync(x => x.Credentials.Email.Equals(email), cancellationToken);

        if (client == null) return false;

        client!.ClaimedRewards.First(x => x.Id.Equals(parsedCommand.RewardId)).IsUsed = true;

        databaseDbContext.Clients.Update(client);

        return true;
    }

    public async Task<bool> DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ClientClaimedRewardModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ClientClaimedRewardModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListUserRewardsQuery;

        IQueryable<ClientClaimedRewardModel> rewards = databaseDbContext.ClientClaimedRewards
            .Include(x => x.Reward)
            .Include(x => x.Client)
            .ThenInclude(x => x.Credentials);

        if (parsedQuery.RewardIds != null && parsedQuery.RewardIds.Any())
            rewards = rewards.Where(x => parsedQuery.RewardIds.Contains(x.RewardId));

        rewards = rewards.Where(x => x.Client.Credentials.Email == parsedQuery.RequestUserInfo.Email);

        return await rewards.ToListAsync(cancellationToken);
    }
}