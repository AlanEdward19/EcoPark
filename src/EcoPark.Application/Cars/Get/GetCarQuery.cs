namespace EcoPark.Application.Cars.Get;

public class GetCarQuery : IQuery
{
    public Guid CarId { get; set; }

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}