namespace EcoPark.Application.ParkingSpaces.Insert;

public class InsertParkingSpaceCommandHandler(IRepository<ParkingSpaceModel> repository) : IHandler<InsertParkingSpaceCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertParkingSpaceCommand command,
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

                    await repository.AddAsync(command, cancellationToken);

                    await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    await repository.UnitOfWork.CommitAsync(cancellationToken);

                    result = new DatabaseOperationResponseViewModel(EOperationStatus.Successful,
                        "Parking space was inserted successfully!");

                    break;

                case EOperationStatus.NotAuthorized:
                    result = new DatabaseOperationResponseViewModel(EOperationStatus.NotAuthorized,
                        "You have no permission to insert this parking space");
                    break;

                case EOperationStatus.Failed:
                    break;

                case EOperationStatus.NotFound:
                    result = new DatabaseOperationResponseViewModel(EOperationStatus.NotFound,
                        "Location not found, check input data");
                    break;
            }
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new DatabaseOperationResponseViewModel(EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}