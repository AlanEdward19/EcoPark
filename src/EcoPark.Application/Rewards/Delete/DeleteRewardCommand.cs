namespace EcoPark.Application.Rewards.Delete;

public record DeleteRewardCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}