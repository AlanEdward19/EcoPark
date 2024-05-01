namespace EcoPark.Application.Employees.Insert.GroupAccess;

public class InsertEmployeeGroupAccessCommand : ICommand
{
    public Guid EmployeeId { get; set; }
    public Guid LocationId { get; set; }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}