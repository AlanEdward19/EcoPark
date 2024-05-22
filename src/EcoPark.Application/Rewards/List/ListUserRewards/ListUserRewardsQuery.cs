namespace EcoPark.Application.Rewards.List.ListUserRewards;

public class ListUserRewardsQuery(IEnumerable<Guid>? rewardIds) : IQuery
{
    public IEnumerable<Guid>? RewardIds { get; private set; } = rewardIds;

    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}