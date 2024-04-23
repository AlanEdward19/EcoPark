namespace EcoPark.Application.Cars.List;

public class ListCarQueryHandler(IRepository<CarModel> repository) : IHandler<ListCarQuery, IEnumerable<CarViewModel>>
{
    public async Task<IEnumerable<CarViewModel>> HandleAsync(ListCarQuery command, CancellationToken cancellationToken)
    {
        var cars = await repository.ListAsync(command, cancellationToken);

        if(cars == null || !cars.Any())
            return Enumerable.Empty<CarViewModel>();

        List<CarViewModel> result = new(cars.Count());

        foreach (var car in cars)
        {
            CarViewModel model = new(car.Plate, car.Type, car.Brand, car.Model, car.Color, car.Year);

            result.Add(model);
        }

        return result;
    }
}