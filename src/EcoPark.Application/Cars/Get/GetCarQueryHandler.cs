﻿namespace EcoPark.Application.Cars.Get;

public class GetCarQueryHandler(IRepository<CarModel> repository) : IHandler<GetCarQuery, CarViewModel?>
{
    public async Task<CarViewModel?> HandleAsync(GetCarQuery command, CancellationToken cancellationToken)
    {
        CarViewModel result = null;
        var car = await repository.GetByIdAsync(command, cancellationToken);

        if (car != null)
            result = new(car.Plate, car.Type, car.Brand, car.Model, car.Color, car.Year);

        return result;
    }
}