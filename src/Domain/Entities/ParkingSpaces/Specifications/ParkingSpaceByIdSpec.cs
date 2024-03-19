using Ardalis.Specification;

namespace Domain.Entities.ParkingSpaces.Specifications;

public class ParkingSpaceByIdSpec : Specification<ParkingSpace>
{
    public ParkingSpaceByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}