namespace EcoPark.Application.Punctuations.Models;

public class PunctuationViewModel(Guid locationId, string locationName, double punctuation)
{
    public Guid LocationId { get; set; } = locationId;
    public string LocationName { get; set; } = locationName;
    public double Punctuation { get; set; } = punctuation;
}