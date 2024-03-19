using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Commons.Enums;
using Domain.Entities.Locations;
using Domain.Entities.Reservations;

namespace Domain.Entities.ParkingSpaces;

public class ParkingSpace
{
    [Key]
    public Guid Id { get; set; }
    public Guid LocationId { get; set; }
    public Guid? ReservationId { get; set; }
    public int Floor { get; set; }
    public string ParkingSpaceName { get; set; }
    public bool IsOccupied { get; set; }
    public bool IsReserved { get; set; }
    public EParkingSpaceType ParkingSpaceType { get; set; }

    [ForeignKey(nameof(LocationId))]
    public virtual Location Location { get; set; }

    [ForeignKey(nameof(ReservationId))]
    public virtual Reservation? Reservation { get; set; }
}