namespace EcoPark.Application.Locations.Update;

public class UpdateLocationCommandHandler(IAggregateRepository<LocationModel> repository) : IHandler<UpdateLocationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateLocationCommand command,
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            await repository.UnitOfWork.StartAsync(cancellationToken);

            var databaseOperationResult = await repository.UpdateAsync(command, cancellationToken);

            if (databaseOperationResult)
            {
                await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                await repository.UnitOfWork.CommitAsync(cancellationToken);

                result = new("Patch", EOperationStatus.Successful, "Location updated successfully");
            }
            else
            {
                await repository.UnitOfWork.RollbackAsync(cancellationToken);
                result = new("Patch", EOperationStatus.Failed, "No Location were found with this id");
            }
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new("Patch", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}