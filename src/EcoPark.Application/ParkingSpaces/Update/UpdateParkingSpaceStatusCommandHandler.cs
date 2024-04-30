namespace EcoPark.Application.ParkingSpaces.Update;

public class UpdateParkingSpaceStatusCommandHandler(IAggregateRepository<ParkingSpaceModel> repository)
    : IHandler<UpdateParkingSpaceStatusCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateParkingSpaceStatusCommand command,
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            if (await repository.CheckChangePermissionAsync(command, cancellationToken))
            {
                await repository.UnitOfWork.StartAsync(cancellationToken);

                var databaseOperationResult = await repository.UpdateAsync(command, cancellationToken);

                if (databaseOperationResult)
                {
                    await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    await repository.UnitOfWork.CommitAsync(cancellationToken);

                    result = new("Patch", EOperationStatus.Successful, "Parking space status updated successfully");
                }
                else
                {
                    await repository.UnitOfWork.RollbackAsync(cancellationToken);
                    result = new("Patch", EOperationStatus.Failed, "No Parking space were found with this id");
                }
            }
            else
                result = new("Patch", EOperationStatus.NotAuthorized, "You have no permission to update this parking space");
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new("Patch", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}