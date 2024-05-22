namespace EcoPark.Application.Employees.Delete.GroupAccess;

public class DeleteEmployeeGroupAccessCommand : ICommand
{
    public Guid LocationId { get; set; }
    public Guid EmployeeId { get; set; }

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}