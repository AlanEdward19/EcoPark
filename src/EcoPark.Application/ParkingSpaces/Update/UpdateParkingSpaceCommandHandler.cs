namespace EcoPark.Application.ParkingSpaces.Update;

public class UpdateParkingSpaceCommandHandler(IRepository<ParkingSpaceModel> repository) : IHandler<UpdateParkingSpaceCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateParkingSpaceCommand command,
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

                    result = new(EOperationStatus.Successful, "Parking space updated successfully");

                    break;

                case EOperationStatus.NotAuthorized:
                    result = new(EOperationStatus.NotAuthorized, "You have no permission to update this parking space");
                    break;

                case EOperationStatus.Failed:
                    break;

                case EOperationStatus.NotFound:
                    result = new(EOperationStatus.NotFound, "Parking space not found");
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