namespace EcoPark.Application.Clients.List;

public class ListClientsQueryHandler(IRepository<ClientModel> repository) : IHandler<ListClientsQuery, IEnumerable<ClientSimplifiedViewModel>>
{
    public async Task<IEnumerable<ClientSimplifiedViewModel>> HandleAsync(ListClientsQuery command, CancellationToken cancellationToken)
    {
        var clients = await repository.ListAsync(command, cancellationToken);

        if (clients == null || !clients.Any())
            return Enumerable.Empty<ClientSimplifiedViewModel>();

        if (command.IncludeCars)
        {
            List<ClientViewModel> result = new(clients.Count());

            foreach (var client in clients)
            {
                IEnumerable<CarViewModel> cars = client.Cars.Select(car =>
                    new CarViewModel(car.Id, car.Plate, car.Type, car.Brand, car.Model, car.Color, car.Year));

                ClientViewModel model = new(client.Credentials.Id, client.Credentials.Email,
                    client.Credentials.FirstName, client.Credentials.LastName, client.Credentials.Image, cars);

                result.Add(model);
            }

            return result;
        }
        else
        {
            List<ClientSimplifiedViewModel> result = new(clients.Count());

            foreach (var client in clients)
            {
                ClientSimplifiedViewModel model = new(client.Credentials.Id, client.Credentials.Email,
                    client.Credentials.FirstName, client.Credentials.LastName, client.Credentials.Image);

                result.Add(model);
            }


            return result;
        }
    }
}