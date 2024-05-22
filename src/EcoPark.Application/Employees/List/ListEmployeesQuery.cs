namespace EcoPark.Application.Employees.List;

public class ListEmployeesQuery : IQuery
{
    public IEnumerable<Guid>? EmployeeIds { get; set; }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}