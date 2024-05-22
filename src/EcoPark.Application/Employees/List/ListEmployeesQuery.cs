namespace EcoPark.Application.Employees.List;

public class ListEmployeesQuery : IQuery
{
    public IEnumerable<Guid>? EmployeeIds { get; set; }

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}