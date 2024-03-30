namespace EcoPark.Application.Reservations.Get;

public class GetReservationQuery
{
    public Guid ReservationId { get; set; }
    public bool IncludeParkingSpace { get; set; }
}