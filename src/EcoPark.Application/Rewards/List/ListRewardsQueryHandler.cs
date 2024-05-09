namespace EcoPark.Application.Rewards.List;

public class ListRewardsQueryHandler(IRepository<RewardModel> repository) : IHandler<ListRewardsQuery, IEnumerable<RewardViewModel>>
{
    public async Task<IEnumerable<RewardViewModel>> HandleAsync(ListRewardsQuery command, CancellationToken cancellationToken)
    {
        List<RewardViewModel> result = command.RewardIds != null && command.RewardIds.Any()
            ? new(command.RewardIds.Count())
            : new();

        var rewards = await repository.ListAsync(command, cancellationToken);

        foreach (var reward in rewards)
        {
            RewardViewModel rewardModel = new(reward.Name, reward.Description, reward.AvailableQuantity,
                reward.RequiredPoints, reward.Image,
                reward.ExpirationDate);

            result.Add(rewardModel);
        }

        return result;
    }
}