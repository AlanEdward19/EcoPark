namespace EcoPark.Domain.ValueObjects;

public class RewardValueObject(RewardModel reward)
{
    public string Name { get; set; } = reward.Name;
    public string Description { get; set; } = reward.Description;
    public int? AvailableQuantity { get; set; } = reward.AvailableQuantity;
    public double RequiredPoints { get; set; } = reward.RequiredPoints;
    public bool IsActive { get; set; } = reward.IsActive;
    public string Url { get; set; } = reward.Url;
    public string Image { get; set; } = reward.Image;
    public DateTime? ExpirationDate { get; set; } = reward.ExpirationDate;

    public void UpdateName(string name)
    {
        if (!string.IsNullOrWhiteSpace(name) && !Name.Equals(name))
            Name = name;
    }

    public void UpdateDescription(string description)
    {
        if (!string.IsNullOrWhiteSpace(description) && !Description.Equals(description))
            Description = description;
    }

    public void UpdateAvailableQuantity(int? availableQuantity)
    {
        AvailableQuantity = availableQuantity;
    }

    public void UpdateRequiredPoints(double? requiredPoints)
    {
        if (requiredPoints is > 0 && !RequiredPoints.Equals(requiredPoints))
            RequiredPoints = requiredPoints.Value;
    }

    public void UpdateIsActive(bool? isActive)
    {
        if (isActive.HasValue && !IsActive.Equals(isActive))
            IsActive = isActive.Value;
    }

    public void UpdateUrl(string url)
    {
        if (!string.IsNullOrWhiteSpace(url) && !Url.Equals(url))
            Url = url;
    }

    public void UpdateImage(string image)
    {
        if (!string.IsNullOrWhiteSpace(image) && !Image.Equals(image))
            Image = image;
    }

    public void UpdateExpirationDate(DateTime? expirationDate)
    {
        ExpirationDate = expirationDate;
    }
}