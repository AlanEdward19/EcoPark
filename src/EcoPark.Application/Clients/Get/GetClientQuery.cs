namespace EcoPark.Application.Clients.Get;

public class GetClientQuery : IQuery
{
    public Guid ClientId { get; set; }
    public bool IncludeCars { get; set; }
}