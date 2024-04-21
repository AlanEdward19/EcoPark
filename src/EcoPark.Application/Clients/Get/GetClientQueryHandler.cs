namespace EcoPark.Application.Clients.Get;

public class GetClientQueryHandler(IAggregateRepository<ClientModel> repository) : IHandler<GetClientQuery, ClientSimplifiedViewModel?>
{
    public async Task<ClientSimplifiedViewModel?> HandleAsync(GetClientQuery command, CancellationToken cancellationToken)
    {
        ClientSimplifiedViewModel result = null;
        var client = await repository.GetByIdAsync(command, cancellationToken);

        if (client != null)
        {
            if (command.IncludeCars)
            {
                IEnumerable<CarViewModel> cars = client.Cars.Select(car =>
                    new CarViewModel(car.Plate, car.Type, car.Brand, car.Model, car.Color, car.Year));

                result = new ClientViewModel(client.Email, client.FirstName, client.LastName, cars);
            }
            else
            {
                result = new ClientSimplifiedViewModel(client.Email, client.FirstName, client.LastName);
            }
        }

        return result;
    }
}