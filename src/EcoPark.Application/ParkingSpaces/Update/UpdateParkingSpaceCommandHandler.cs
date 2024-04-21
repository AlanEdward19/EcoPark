namespace EcoPark.Application.ParkingSpaces.Update;

public class UpdateParkingSpaceCommandHandler(IAggregateRepository<ParkingSpaceModel> repository) : IHandler<UpdateParkingSpaceCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateParkingSpaceCommand command,
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

                result = new("Patch", EOperationStatus.Successful, "Parking space updated successfully");
            }
            else
            {
                await repository.UnitOfWork.RollbackAsync(cancellationToken);
                result = new("Patch", EOperationStatus.Failed, "No Parking space were found with this id");
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