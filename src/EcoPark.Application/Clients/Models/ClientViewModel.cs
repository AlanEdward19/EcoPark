namespace EcoPark.Application.Clients.Models;

public class ClientViewModel(Guid id, string email, string firstName, string lastName, string? imageUrl, IEnumerable<CarViewModel>? cars)
    : ClientSimplifiedViewModel(id, email, firstName, lastName, imageUrl)
{
    public IEnumerable<CarViewModel>? Cars { get; private set; } = cars;
}