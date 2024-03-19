using Ardalis.Specification;

namespace Domain.Entities.Locations.Specifications;

public class LocationByIdSpec : Specification<Location>
{
    public LocationByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}