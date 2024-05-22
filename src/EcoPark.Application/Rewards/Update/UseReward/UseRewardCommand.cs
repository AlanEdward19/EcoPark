namespace EcoPark.Application.Rewards.Update.UseReward;

public class UseRewardCommand(Guid rewardId) : ICommand
{
    public Guid RewardId { get; private set; } = rewardId;

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}