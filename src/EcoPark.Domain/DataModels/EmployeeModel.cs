using EcoPark.Domain.ValueObjects;

namespace EcoPark.Domain.DataModels;

public class EmployeeModel(Guid? administratorId, Guid credentialsId)
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? AdministratorId { get; set; } = administratorId;
    public Guid CredentialsId { get; set; } = credentialsId;

    [ForeignKey(nameof(AdministratorId))]
    public virtual EmployeeModel? Administrator { get; set; }

    [ForeignKey(nameof(CredentialsId))]
    public virtual CredentialsModel Credentials { get; set; }
    public virtual IEnumerable<GroupAccessModel> GroupAccesses { get; set; }
    public virtual IEnumerable<EmployeeModel> Employees { get; set; }

    public void SetCredentials(CredentialsModel credentials)
    {
        Credentials = credentials;
    }

    public void UpdateBasedOnValueObject(EmployeeValueObject employeeValueObject)
    {
        Credentials.Email = employeeValueObject.Email;
        Credentials.Password = employeeValueObject.Password;
        Credentials.FirstName = employeeValueObject.FirstName;
        Credentials.LastName = employeeValueObject.LastName;
        Credentials.UserType = employeeValueObject.UserType;
        Credentials.UpdatedAt = DateTime.Now;
    }
}