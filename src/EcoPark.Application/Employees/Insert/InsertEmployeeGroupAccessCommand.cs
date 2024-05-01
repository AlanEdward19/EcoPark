namespace EcoPark.Application.Employees.Insert;

public class InsertEmployeeGroupAccessCommand : ICommand
{
    public Guid EmployeeId { get; set; }
    public Guid LocationId { get; set; }

    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}