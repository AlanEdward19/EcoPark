namespace EcoPark.Application.ParkingSpaces.Delete;

public class DeleteParkingSpaceCommandHandler(DatabaseDbContext databaseDbContext) : IHandler<DeleteParkingSpaceCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(DeleteParkingSpaceCommand command,
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            ParkingSpaceModel? parkingSpaceModel = await databaseDbContext.ParkingSpaces
                .FirstOrDefaultAsync(ps => ps.Id == command.Id, cancellationToken);

            if (parkingSpaceModel != null)
            {
                databaseDbContext.ParkingSpaces.Remove(parkingSpaceModel);

                await databaseDbContext.SaveChangesAsync(cancellationToken);

                result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Successful,
                    "Parking space was deleted successfully!");
            }
            else
                result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed,
                    "No Parking space were found with this id");
        }
        catch (Exception e)
        {
            result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}