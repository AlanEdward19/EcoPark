namespace EcoPark.Application.Rewards.Get;

public class GetRewardQueryHandler(IRepository<RewardModel> repository) : IHandler<GetRewardQuery, RewardViewModel?>
{
    public async Task<RewardViewModel?> HandleAsync(GetRewardQuery command, CancellationToken cancellationToken)
    {
        RewardViewModel? result = null;
        var reward = await repository.GetByIdAsync(command, cancellationToken);

        if (reward != null)
            result = new(reward.Id, reward.Name, reward.Description, reward.AvailableQuantity,
                reward.RequiredPoints, reward.Url, reward.Image, reward.ExpirationDate);

        return result;
    }
}