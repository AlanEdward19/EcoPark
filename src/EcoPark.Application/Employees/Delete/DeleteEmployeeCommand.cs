namespace EcoPark.Application.Employees.Delete;

public record DeleteEmployeeCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}