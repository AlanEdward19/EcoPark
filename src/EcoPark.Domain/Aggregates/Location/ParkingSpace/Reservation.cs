namespace EcoPark.Domain.Aggregates.Location.ParkingSpace;

public class Reservation(ReservationModel reservationModel)
{
    public Guid Id { get; private set; } = reservationModel.Id;
    public Guid ClientId { get; private set; } = reservationModel.ClientId;
    public Guid CardId { get; private set; } = reservationModel.CardId;
    public string ReservationCode { get; private set; } = string.IsNullOrWhiteSpace(reservationModel.ReservationCode) ? GenerateReservationCode() : reservationModel.ReservationCode;
    public EReservationStatus Status { get; private set; } = reservationModel.Status;
    public DateTime ReservationDate { get; private set; } = reservationModel.ReservationDate;
    public DateTime ExpirationDate { get; private set; } = reservationModel.ExpirationDate;

    public static string GenerateReservationCode()
    {
        return "";
    }

    public void ChangeStatus(EReservationStatus status) => Status = status;
}