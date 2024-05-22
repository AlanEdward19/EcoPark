namespace EcoPark.Application.Reservations.Models;

public class ReservationSimplifiedViewModel(Guid id, Guid? carId, Guid? clientId, string reservationCode, double punctuation, EReservationStatus status, 
    DateTime reservationDate, DateTime expirationDate)
{
    public Guid Id { get; private set; } = id;
    public Guid? CarId { get; private set; } = carId;
    public Guid? ClientId { get; private set; } = clientId;
    public string ReservationCode { get; private set; } = reservationCode;
    public double Punctuation { get; private set; } = punctuation;
    public string Status { get; private set; } = status.ToString();
    public DateTime ReservationDate { get; private set; } = reservationDate;
    public DateTime ExpirationDate { get; private set; } = expirationDate;
}