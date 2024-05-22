namespace EcoPark.Application.Rewards.Get;

public class GetRewardQuery : IQuery
{
    public Guid RewardId { get; set; }

    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}