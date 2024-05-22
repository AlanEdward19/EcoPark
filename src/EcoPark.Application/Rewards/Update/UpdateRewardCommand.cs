using Microsoft.AspNetCore.Http;

namespace EcoPark.Application.Rewards.Update;

public class UpdateRewardCommand : ICommand
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? AvailableQuantity { get; set; }
    public double? RequiredPoints { get; set; }
    public bool? IsActive { get; set; }
    public string? Url { get; set; }
    public MemoryStream? Image { get; private set; }
    public string? ImageFileName { get; private set; }
    public DateTime? ExpirationDate { get; set; }

    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }

    public async Task SetImage(IFormFile? image, string? imageFileName, CancellationToken cancellationToken)
    {
        if (image != null)
        {
            Image = new();

            await image.CopyToAsync(Image, cancellationToken);
            ImageFileName = imageFileName;
        }
    }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}