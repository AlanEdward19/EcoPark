namespace EcoPark.Application.Employees.Insert.GroupAccess;

public class InsertEmployeeGroupAccessCommand : ICommand
{
    public Guid EmployeeId { get; set; }
    public Guid LocationId { get; set; }

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}