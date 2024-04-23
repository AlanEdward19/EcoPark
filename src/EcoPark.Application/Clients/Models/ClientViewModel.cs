namespace EcoPark.Application.Clients.Models;

public class ClientViewModel(Guid id, string email, string firstName, string lastName, IEnumerable<CarViewModel>? cars) 
    : ClientSimplifiedViewModel(id, email, firstName, lastName)
{
    public IEnumerable<CarViewModel>? Cars { get; private set; } = cars;
}