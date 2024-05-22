namespace EcoPark.Application.Cars.List;

public class ListCarQuery(IEnumerable<Guid> carIds) : IQuery
{
    public IEnumerable<Guid> CarIds { get; private set; } = carIds;

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}