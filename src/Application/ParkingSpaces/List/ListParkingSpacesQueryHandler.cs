namespace Application.ParkingSpaces.List;

public class ListParkingSpacesQueryHandler : IHandler<IEnumerable<Guid>?, IEnumerable<ParkingSpace>>
{
    public async Task<IEnumerable<ParkingSpace>> HandleAsync(IEnumerable<Guid>? command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}