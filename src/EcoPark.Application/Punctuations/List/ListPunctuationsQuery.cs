namespace EcoPark.Application.Punctuations.List;

public class ListPunctuationsQuery: IQuery
{
    public IEnumerable<Guid>? LocationIds { get; set; }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}