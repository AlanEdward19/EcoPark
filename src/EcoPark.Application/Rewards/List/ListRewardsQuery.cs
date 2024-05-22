namespace EcoPark.Application.Rewards.List;

public class ListRewardsQuery(Guid? locationId, IEnumerable<Guid>? rewardIds) : IQuery
{
    public Guid? LocationId { get; private set; } = locationId;
    public IEnumerable<Guid>? RewardIds { get; private set; } = rewardIds;

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}