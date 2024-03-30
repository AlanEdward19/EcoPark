namespace EcoPark.Domain.DataModels;

public class ReservationModel : BaseDataModel
{
    public Guid ParkingSpaceId { get; private set; }
    public Guid CardId { get; private set; }
    public Guid ClientId { get; private set; }
    public string ReservationCode { get; private set; }
    public EReservationStatus Status { get; private set; }
    public DateTime ReservationDate { get; private set; }
    public DateTime ExpirationDate { get; private set; }

    [ForeignKey(nameof(ParkingSpaceId))]
    public virtual ParkingSpaceModel ParkingSpace { get; set; }

    [ForeignKey(nameof(ClientId))]
    public virtual ClientModel Client { get; set; }

    public string GenerateReservationCode()
    {
        return "";
    }

    public ReservationModel(Guid parkingSpaceId, Guid cardId, Guid clientId, DateTime reservationDate)
    {
        ParkingSpaceId = parkingSpaceId;
        CardId = cardId;
        ClientId = clientId;
        ReservationCode = GenerateReservationCode();
        Status = EReservationStatus.Created;
        ReservationDate = reservationDate;
        ExpirationDate = reservationDate.AddMinutes(15);
    }
}