namespace EcoPark.Application.Reservations.Get;

public class GetReservationQuery : IQuery
{
    public Guid ReservationId { get; set; }
    public bool IncludeParkingSpace { get; set; }
}