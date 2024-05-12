namespace EcoPark.Application.Rewards.Insert.RedeemReward;

public class RedeemRewardCommand(Guid rewardId) : ICommand
{
    public Guid RewardId { get; private set; } = rewardId;

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}