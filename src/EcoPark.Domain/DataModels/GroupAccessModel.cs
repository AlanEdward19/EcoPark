namespace EcoPark.Domain.DataModels;

public class GroupAccessModel(Guid locationId, Guid employeeId) : BaseDataModel
{
    public Guid LocationId { get; private set; } = locationId;
    public Guid EmployeeId { get; private set; } = employeeId;

    [ForeignKey(nameof(LocationId))]
    public virtual LocationModel Location { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public virtual EmployeeModel Employee { get; set; }
}