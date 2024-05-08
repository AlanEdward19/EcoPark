using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace EcoPark.Application.Rewards.Insert;

public class InsertRewardCommand : ICommand
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? AvailableQuantity { get; set; }
    public double? RequiredPoints { get; set; }
    public bool? IsActive { get; set; }
    public string? Url { get; set; }
    public MemoryStream? Image { get; private set; }
    public string? ImageFileName { get; private set; }
    public DateTime? ExpirationDate { get; set; }
    public Guid? LocationId { get; set; }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public async Task SetImage(IFormFile? image, string imageFileName, CancellationToken cancellationToken)
    {
        Image = new();

        await image.CopyToAsync(Image, cancellationToken);
        ImageFileName = imageFileName;
    }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}