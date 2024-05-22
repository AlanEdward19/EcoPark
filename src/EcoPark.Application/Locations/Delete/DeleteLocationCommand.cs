namespace EcoPark.Application.Locations.Delete;

public record DeleteLocationCommand : DeleteEntityCommand, ICommand 
{
    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}