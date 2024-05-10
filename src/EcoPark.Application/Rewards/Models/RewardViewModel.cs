namespace EcoPark.Application.Rewards.Models;

public class RewardViewModel(Guid id, string name, string description, int? quantity, double pointCost, string url, string imageUrl, DateTime? expirationDate) : RewardSimplifiedViewModel(name, description, url, imageUrl, expirationDate)
{
    public Guid Id { get; private set; } = id;
    public int? Quantity { get; private set; } = quantity;
    public double PointCost { get; private set; } = pointCost;
}