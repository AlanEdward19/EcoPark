using EcoPark.Domain.ValueObjects;

namespace EcoPark.Domain.DataModels.Employee.Location;

public class RewardModel : BaseDataModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int? AvailableQuantity { get; set; }
    public double RequiredPoints { get; set; }
    public bool IsActive { get; set; }
    public string Url { get; set; }
    public string Image { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public Guid LocationId { get; set; }

    [ForeignKey(nameof(LocationId))]
    public virtual LocationModel Location { get; set; }

    public RewardModel() { }

    public RewardModel(string name, string description, int? availableQuantity, double requiredPoints, string url, string image, bool isActive, 
        DateTime? expirationDate, Guid locationId)
    {
        Name = name;
        Description = description;
        AvailableQuantity = availableQuantity;
        RequiredPoints = requiredPoints;
        Url = url;
        Image = image;
        IsActive = isActive;
        ExpirationDate = expirationDate;
        LocationId = locationId;
    }

    public void UpdateBasedOnValueObject(RewardValueObject rewardValueObject)
    {
        Name = rewardValueObject.Name;
        Description = rewardValueObject.Description;
        AvailableQuantity = rewardValueObject.AvailableQuantity;
        RequiredPoints = rewardValueObject.RequiredPoints;
        IsActive = rewardValueObject.IsActive;
        Url = rewardValueObject.Url;
        Image = rewardValueObject.Image;
        ExpirationDate = rewardValueObject.ExpirationDate;
        UpdatedAt = DateTime.Now;
    }
}