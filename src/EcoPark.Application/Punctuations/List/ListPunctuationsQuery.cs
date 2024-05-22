namespace EcoPark.Application.Punctuations.List;

public class ListPunctuationsQuery: IQuery
{
    public IEnumerable<Guid>? LocationIds { get; set; }

    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}