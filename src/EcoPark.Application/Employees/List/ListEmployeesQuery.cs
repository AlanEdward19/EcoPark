namespace EcoPark.Application.Employees.List;

public class ListEmployeesQuery : IQuery
{
    public IEnumerable<Guid>? EmployeeIds { get; set; }

    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}