namespace EcoPark.Domain.DataModels;

public class GroupAccessModel(Guid locationId) : BaseDataModel
{
    public Guid LocationId { get; private set; } = locationId;

    [ForeignKey(nameof(LocationId))]
    public virtual LocationModel Location { get; set; }
}