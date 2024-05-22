using System.Xml.Linq;

namespace EcoPark.Application.Rewards.Models;

public class RewardSimplifiedViewModel(string name, string description, string url, string imageUrl, DateTime? expirationDate)
{
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public string Url { get; private set; } = url;
    public string ImageUrl { get; private set; } = imageUrl;
    public DateTime? ExpirationDate { get; private set; } = expirationDate;
}