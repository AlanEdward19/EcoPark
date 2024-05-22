namespace EcoPark.Application.Cars.List;

public class ListCarQuery(IEnumerable<Guid> carIds) : IQuery
{
    public IEnumerable<Guid> CarIds { get; private set; } = carIds;

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}