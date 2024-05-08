namespace EcoPark.Domain.DataModels.Client;

public class ClientClaimedRewardModel : BaseDataModel
{
    public Guid ClientId { get; set; }
    public Guid RewardId { get; set; }
    public bool IsUsed { get; set; }

    [ForeignKey(nameof(ClientId))]
    public virtual ClientModel Client { get; set; }

    [ForeignKey(nameof(RewardId))]
    public virtual RewardModel Reward { get; set; }
}