﻿namespace EcoPark.Application.Reservations.List;

public class ListReservationQuery(IEnumerable<Guid> reservationIds, bool includeParkingSpace) : IQuery
{
    public IEnumerable<Guid> ReservationIds { get; private set; } = reservationIds;
    public bool IncludeParkingSpace { get; private set; } = includeParkingSpace;

    [JsonIgnore]
    public RequestUserInfoValueObject? RequestUserInfo { get; private set; }
    public void SetRequestUserInfo(RequestUserInfoValueObject information)
    {
        RequestUserInfo = information;
    }
}