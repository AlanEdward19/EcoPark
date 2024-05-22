namespace EcoPark.Application.Reservations.Get;

public class GetReservationQuery : IQuery
{
    public Guid ReservationId { get; set; }
    public bool IncludeParkingSpace { get; set; }

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}