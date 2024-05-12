namespace EcoPark.Application.Rewards.Update.UseReward;

public class UseRewardCommand(Guid rewardId) : ICommand
{
    public Guid RewardId { get; private set; } = rewardId;

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}