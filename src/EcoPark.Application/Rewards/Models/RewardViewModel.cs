namespace EcoPark.Application.Rewards.Models;

public class RewardViewModel(string name, string description, int? quantity, double pointCost, string imageUrl, DateTime? expirationDate)
{
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public int? Quantity { get; private set; } = quantity;
    public double PointCost { get; private set; } = pointCost;
    public string ImageUrl { get; private set; } = imageUrl;
    public DateTime? ExpirationDate { get; private set; } = expirationDate;
}