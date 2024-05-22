namespace EcoPark.Application.Cars.Update;

public class UpdateCarCommandHandler(IRepository<CarModel> repository) : IHandler<UpdateCarCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateCarCommand command,
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result = new(EOperationStatus.Failed, "");
        try
        {
            EOperationStatus status = await repository.CheckChangePermissionAsync(command, cancellationToken);

            switch (status)
            {
                case EOperationStatus.Successful:
                    await repository.UnitOfWork.StartAsync(cancellationToken);

                    await repository.UpdateAsync(command, cancellationToken);

                    await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    await repository.UnitOfWork.CommitAsync(cancellationToken);

                    result = new(EOperationStatus.Successful, "Car updated successfully");

                    break;

                case EOperationStatus.NotAuthorized:
                    result = new(EOperationStatus.NotAuthorized, "You have no permission to update this car");
                    break;

                case EOperationStatus.Failed:
                    break;

                case EOperationStatus.NotFound:
                    result = new(EOperationStatus.NotFound, "No Car were found with this id");
                    break;
            }
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new( EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}