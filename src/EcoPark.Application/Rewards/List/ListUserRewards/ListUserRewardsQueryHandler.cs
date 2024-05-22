namespace EcoPark.Application.Rewards.List.ListUserRewards;

public class ListUserRewardsQueryHandler(IRepository<ClientClaimedRewardModel> repository) : IHandler<ListUserRewardsQuery, IEnumerable<UserRewardViewModel>>
{
    public async Task<IEnumerable<UserRewardViewModel>> HandleAsync(ListUserRewardsQuery command, CancellationToken cancellationToken)
    {
        List<UserRewardViewModel> result = command.RewardIds != null && command.RewardIds.Any()
            ? new(command.RewardIds.Count())
            : new();

        var rewards = await repository.ListAsync(command, cancellationToken);

        foreach (var reward in rewards)
        {
            UserRewardViewModel rewardModel = new(reward.Id, reward.Reward.Name, reward.Reward.Description,
                reward.IsUsed, reward.Reward.Url, reward.Reward.Image, reward.Reward.ExpirationDate);

            result.Add(rewardModel);
        }

        return result;
    }
}