namespace EcoPark.Application.Employees.Get;

public class GetEmployeeQuery : IQuery
{
    public Guid EmployeeId { get; set; }

    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}