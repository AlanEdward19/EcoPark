namespace EcoPark.Application.Punctuations.Get;

public class GetPunctuationQuery: IQuery
{
    public Guid LocationId { get; set; }
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}