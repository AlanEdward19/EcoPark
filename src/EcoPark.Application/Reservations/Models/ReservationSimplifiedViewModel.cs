namespace EcoPark.Application.Reservations.Models;

public class ReservationSimplifiedViewModel(Guid cardId, Guid clientId, string reservationCode, EReservationStatus status, DateTime reservationDate, DateTime expirationDate)
{
    public Guid CardId { get; private set; } = cardId;
    public Guid ClientId { get; private set; } = clientId;
    public string ReservationCode { get; private set; } = reservationCode;
    public string Status { get; private set; } = status.ToString();
    public DateTime ReservationDate { get; private set; } = reservationDate;
    public DateTime ExpirationDate { get; private set; } = expirationDate;
}