namespace EcoPark.Application.Punctuation.Get;

public class GetPunctuationQuery: IQuery
{
    public Guid LocationId { get; set; }
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}