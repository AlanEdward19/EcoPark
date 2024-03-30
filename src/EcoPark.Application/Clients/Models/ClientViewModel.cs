namespace EcoPark.Application.Clients.Models;

public class ClientViewModel(string email, string firstName, string lastName, IEnumerable<CarViewModel>? cars) : ClientSimplifiedViewModel(email, firstName, lastName)
{
    public IEnumerable<CarViewModel>? Cars { get; private set; } = cars;
}