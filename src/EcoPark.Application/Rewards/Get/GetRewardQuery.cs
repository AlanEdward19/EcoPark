namespace EcoPark.Application.Rewards.Get;

public class GetRewardQuery : IQuery
{
    public Guid RewardId { get; set; }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}