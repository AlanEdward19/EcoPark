namespace EcoPark.Application.Employees.Delete.GroupAccess;

public class DeleteEmployeeGroupAccessCommand : ICommand
{
    public Guid LocationId { get; set; }
    public Guid EmployeeId { get; set; }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}