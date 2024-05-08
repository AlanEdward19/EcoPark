namespace EcoPark.Domain.Aggregates.Location.ParkingSpace;

public class Reservation(ReservationModel reservationModel)
{
    public Guid Id { get; private set; } = reservationModel.Id;
    public Guid ClientId { get; private set; } = reservationModel.ClientId!.Value;
    public Guid CarId { get; private set; } = reservationModel.CarId!.Value;
    public string ReservationCode { get; private set; } = string.IsNullOrWhiteSpace(reservationModel.ReservationCode)
        ? GenerateReservationCode()
        : reservationModel.ReservationCode;
    public EReservationStatus Status { get; private set; } = reservationModel.Status;
    public DateTime ReservationDate { get; private set; } = reservationModel.ReservationDate;
    public DateTime ExpirationDate { get; private set; } = reservationModel.ExpirationDate;

    public static string GenerateReservationCode()
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static double CalculatePunctuation(DateTime reservationDate, DateTime? lastReservationDate, ECarType carType, double reservationTax)
    {
        double frequency = 1;

        if (lastReservationDate != null)
            frequency = 100d/((reservationDate - lastReservationDate.Value).TotalDays);

        double sustainability = carType is ECarType.Electric or ECarType.Hybrid ? 20d : 0;

        return double.Round(((reservationTax == 0 ? 1 : reservationTax) * frequency) + sustainability, 2);
    }

    public void ChangeStatus(EReservationStatus status) => Status = status;

    public void ChangeReservationDate(DateTime reservationDate, int reservationGraceInMinutes)
    {
        ReservationDate = reservationDate;
        ExpirationDate = reservationDate.AddMinutes(reservationGraceInMinutes);
    }
}