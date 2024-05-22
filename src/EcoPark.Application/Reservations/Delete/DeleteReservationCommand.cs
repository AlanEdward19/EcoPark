namespace EcoPark.Application.Reservations.Delete;

public record DeleteReservationCommand : DeleteEntityCommand, ICommand
{
    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }

    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}