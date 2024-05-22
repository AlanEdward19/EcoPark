namespace EcoPark.Application.Locations.Update;

public class UpdateLocationCommandHandler(IRepository<LocationModel> repository) : IHandler<UpdateLocationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateLocationCommand command,
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result = new(EOperationStatus.Failed, "AAAAA");

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

                    result = new(EOperationStatus.Successful, "Location updated successfully");

                    break;

                case EOperationStatus.NotAuthorized:
                    result = new(EOperationStatus.NotAuthorized, "You have no permission to update this location");
                    break;

                case EOperationStatus.NotFound:
                    result = new(EOperationStatus.NotFound, "Location wasn't found");
                    break;
            }
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new(EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}