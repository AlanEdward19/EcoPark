namespace EcoPark.Application.Employees.Get;

public class GetEmployeeQuery : IQuery
{
    public Guid EmployeeId { get; set; }

    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}