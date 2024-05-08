namespace EcoPark.Domain.DataModels.Employee.Location.ParkingSpace;

public class ReservationModel : BaseDataModel
{
    public Guid ParkingSpaceId { get; private set; }
    public Guid? ClientId { get; private set; }
    public Guid? CarId { get; private set; }
    public string ReservationCode { get; private set; }
    public double Punctuation { get; private set; }
    public EReservationStatus Status { get; private set; }
    public DateTime ReservationDate { get; private set; }
    public DateTime ExpirationDate { get; private set; }

    [ForeignKey(nameof(ParkingSpaceId))]
    public virtual ParkingSpaceModel ParkingSpace { get; set; }

    [ForeignKey(nameof(ClientId))]
    public virtual ClientModel? Client { get; set; }

    [ForeignKey(nameof(CarId))]
    public virtual CarModel? Car { get; set; }

    public ReservationModel() { }

    public ReservationModel(Guid parkingSpaceId, Guid clientId, Guid carId, DateTime reservationDate, string reservationCode, int reservationGraceInMinutes, double punctuation)
    {
        ParkingSpaceId = parkingSpaceId;
        ClientId = clientId;
        CarId = carId;
        ReservationCode = reservationCode;
        Status = EReservationStatus.Created;
        ReservationDate = reservationDate;
        ExpirationDate = reservationDate.AddMinutes(reservationGraceInMinutes);
        Punctuation = punctuation;
    }

    public ReservationModel(Guid parkingSpaceId, Guid clientId, Guid carId, DateTime reservationDate, string reservationCode, int reservationGraceInMinutes, EReservationStatus status, double punctuation)
    {
        ParkingSpaceId = parkingSpaceId;
        ClientId = clientId;
        CarId = carId;
        ReservationCode = reservationCode;
        Status = status;
        ReservationDate = reservationDate;
        ExpirationDate = reservationDate.AddMinutes(reservationGraceInMinutes);
        Punctuation = punctuation;
    }

    public void UpdateBasedOnValueObject(Reservation reservation)
    {
        Status = reservation.Status;
        ReservationDate = reservation.ReservationDate;
        ExpirationDate = reservation.ExpirationDate;
        UpdatedAt = DateTime.Now;
    }
}