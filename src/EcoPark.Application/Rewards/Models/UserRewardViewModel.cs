namespace EcoPark.Application.Rewards.Models;

public class UserRewardViewModel(Guid id, string name, string description, bool isUsed, string url, string imageUrl, DateTime? expirationDate) : RewardSimplifiedViewModel(name, description, url, imageUrl, expirationDate)
{
    public Guid Id { get; private set; } = id;
    public bool IsUsed { get; private set; } = isUsed;
}