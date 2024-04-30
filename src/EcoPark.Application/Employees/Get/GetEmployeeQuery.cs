namespace EcoPark.Application.Employees.Get;

public class GetEmployeeQuery : IQuery
{
    public Guid EmployeeId { get; set; }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}