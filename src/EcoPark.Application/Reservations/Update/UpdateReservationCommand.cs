namespace EcoPark.Application.Reservations.Update;

public class UpdateReservationCommand(DateTime? reservationDate) : ICommand
{
    public Guid ReservationId { get; private set; }
    public DateTime? ReservationDate { get; private set; } = reservationDate;

    public void SetReservationId(Guid reservationId)
    {
        ReservationId = reservationId;
    }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}