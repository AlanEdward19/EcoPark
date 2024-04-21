namespace EcoPark.Application.ParkingSpaces.Insert;

public class InsertParkingSpaceCommandHandler(IAggregateRepository<ParkingSpaceModel> repository) : IHandler<InsertParkingSpaceCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertParkingSpaceCommand command, 
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            await repository.UnitOfWork.StartAsync(cancellationToken);

            var databaseOperationResult = await repository.AddAsync(command, cancellationToken);

            if (databaseOperationResult)
            {
                await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                await repository.UnitOfWork.CommitAsync(cancellationToken);

                result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Successful,
                    "Parking space was inserted successfully!");
            }
            else
            {
                await repository.UnitOfWork.RollbackAsync(cancellationToken);
                result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, "Parking space was not inserted!");
            }
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}