﻿namespace EcoPark.Application.Reservations.Get;

public class GetReservationQuery : IQuery
{
    public Guid ReservationId { get; set; }
    public bool IncludeParkingSpace { get; set; }

    [JsonIgnore]
    public (string Email, EUserType UserType) RequestUserInfo { get; private set; }
    public void SetRequestUserInfo((string email, EUserType userType) information)
    {
        RequestUserInfo = information;
    }
}