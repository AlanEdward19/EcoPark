namespace EcoPark.Application.Reservations.Update.Status;
public class UpdateReservationStatusCommand : ICommand
{
    public string? ReservationCode { get; private set; }
    public Guid? ReservationId { get; private set; }
    public EReservationStatus Status { get; private set; }

    public void SetReservationStatus(EReservationStatus status)
    {
        Status = status;
    }

    public void SetReservationId(Guid reservationId)
    {
        ReservationId = reservationId;
    }

    public void SetReservationCode(string reservationCode)
    {
        ReservationCode = reservationCode.ToUpper();
    }

    [JsonIgnore]
    public RequestUserInfoValueObject RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}
