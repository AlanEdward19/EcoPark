namespace EcoPark.Application.Rewards.List.ListUserRewards;

public class ListUserRewardsQuery(IEnumerable<Guid>? rewardIds) : IQuery
{
    public IEnumerable<Guid>? RewardIds { get; private set; } = rewardIds;

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}