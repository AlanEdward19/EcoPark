namespace EcoPark.Application.Cars.Get;

public class GetCarQuery : IQuery
{
    public Guid CarId { get; set; }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}