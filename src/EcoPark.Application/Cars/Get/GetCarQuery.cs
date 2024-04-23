namespace EcoPark.Application.Cars.Get;

public class GetCarQuery : IQuery
{
    public Guid CarId { get; set; }

    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}