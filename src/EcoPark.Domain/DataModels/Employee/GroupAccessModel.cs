namespace EcoPark.Domain.DataModels.Employee;

public class GroupAccessModel
{
    public Guid LocationId { get; private set; }
    public Guid EmployeeId { get; private set; }

    public virtual EmployeeModel Employee { get; set; }
    public virtual LocationModel Location { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public GroupAccessModel(Guid locationId, Guid employeeId)
    {
        DateTime now = DateTime.Now;

        LocationId = locationId;
        EmployeeId = employeeId;
        CreatedAt = now;
        UpdatedAt = now;
    }
}