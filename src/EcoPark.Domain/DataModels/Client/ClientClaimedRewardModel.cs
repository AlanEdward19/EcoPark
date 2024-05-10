namespace EcoPark.Domain.DataModels.Client;

public class ClientClaimedRewardModel(Guid clientId, Guid rewardId, bool isUsed) : BaseDataModel
{
    public Guid ClientId { get; set; } = clientId;
    public Guid RewardId { get; set; } = rewardId;
    public bool IsUsed { get; set; } = isUsed;

    [ForeignKey(nameof(ClientId))]
    public virtual ClientModel Client { get; set; }

    [ForeignKey(nameof(RewardId))]
    public virtual RewardModel Reward { get; set; }
}