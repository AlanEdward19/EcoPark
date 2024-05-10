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
                    new CarViewModel(car.Id, car.Plate, car.Type, car.Brand, car.Model, car.Color, car.Year));

                result = new ClientViewModel(client.Credentials.Id, client.Credentials.Email,
                    client.Credentials.FirstName, client.Credentials.LastName, client.Credentials.Image, cars);
            }
            else
                result = new ClientSimplifiedViewModel(client.Credentials.Id, client.Credentials.Email,
                    client.Credentials.FirstName, client.Credentials.LastName, client.Credentials.Image);
        }

        return result;
    }
}