namespace EcoPark.Domain.Commons.Enums;

public enum EReservationStatus
{
    Created, //status inicial
    Confirmed, //se a location tiver valor maior que 0 será utilizado gateway de pagamento, implementado
    Arrived, //implementado
    Completed, //com erro
    Cancelled,// implementado
    Expired // precisa implementar via timer trigger
}