namespace Domain.Entities.Reservations;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid ParkingSpaceId { get; set; }
    public Guid LocationId { get; set; }
    public Guid CardId { get; set; }
}