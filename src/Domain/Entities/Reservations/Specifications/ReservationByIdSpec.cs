using Ardalis.Specification;

namespace Domain.Entities.Reservations.Specifications;

public class ReservationByIdSpec : Specification<Reservation>
{
    public ReservationByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}