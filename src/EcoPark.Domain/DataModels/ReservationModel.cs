namespace EcoPark.Domain.DataModels;

public class ReservationModel : BaseDataModel
{
    public Guid ParkingSpaceId { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid CarId { get; private set; }
    public string ReservationCode { get; private set; }
    public EReservationStatus Status { get; private set; }
    public DateTime ReservationDate { get; private set; }
    public DateTime ExpirationDate { get; private set; }

    [ForeignKey(nameof(ParkingSpaceId))]
    public virtual ParkingSpaceModel ParkingSpace { get; set; }

    [ForeignKey(nameof(ClientId))]
    public virtual ClientModel Client { get; set; }

    [ForeignKey(nameof(CarId))]
    public virtual CarModel Car { get; set; }

    public ReservationModel(Guid parkingSpaceId, Guid clientId, Guid carId, DateTime reservationDate, string reservationCode)
    {
        ParkingSpaceId = parkingSpaceId;
        ClientId = clientId;
        CarId = carId;
        ReservationCode = reservationCode;
        Status = EReservationStatus.Created;
        ReservationDate = reservationDate;
        ExpirationDate = reservationDate.AddMinutes(15);
    }

    public ReservationModel()
    {
        
    }

    public void UpdateBasedOnValueObject(Reservation reservation)
    {
        Status = reservation.Status;
        ReservationDate = reservation.ReservationDate;
        ExpirationDate = reservation.ExpirationDate;
    }
}