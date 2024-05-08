namespace EcoPark.Domain.DataModels;

public class PunctuationModel : BaseDataModel
{
    public PunctuationModel() { }

    public PunctuationModel(Guid clientId, Guid locationId, double punctuation)
    {
        ClientId = clientId;
        LocationId = locationId;
        Punctuation = punctuation;
    }

    public Guid ClientId { get; set; }
    public Guid LocationId { get; set; }
    public double Punctuation { get; set; }

    [ForeignKey(nameof(ClientId))]
    public virtual ClientModel Client { get; set; }

    [ForeignKey(nameof(LocationId))]
    public virtual LocationModel Location { get; set; }
}