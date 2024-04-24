namespace EcoPark.Application.Reservations.Insert;

public class InsertReservationCommand(Guid? parkingSpaceId, Guid? carId, DateTime? reservationDate)
    : ICommand
{
    public Guid? ParkingSpaceId { get; private set; } = parkingSpaceId;
    public Guid? CarId { get; private set; } = carId;
    public DateTime? ReservationDate { get; private set; } = reservationDate;

    [JsonIgnore]
    public (string Email, string UserType) RequestUserInfo { get; private set; }

    public void SetRequestUserInfo((string email, string userType) information)
    {
        RequestUserInfo = information;
    }
}