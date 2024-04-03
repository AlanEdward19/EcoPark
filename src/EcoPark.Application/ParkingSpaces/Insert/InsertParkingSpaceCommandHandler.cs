namespace EcoPark.Application.ParkingSpaces.Insert;

public class InsertParkingSpaceCommandHandler(DatabaseDbContext databaseDbContext) : IHandler<InsertParkingSpaceCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertParkingSpaceCommand command, 
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            ParkingSpaceModel parkingSpaceModel = new(command.LocationId!.Value, command.Floor!.Value, command.ParkingSpaceName!,
                command.IsOccupied!.Value, command.Type!.Value);

            await databaseDbContext.ParkingSpaces.AddAsync(parkingSpaceModel, cancellationToken);

            await databaseDbContext.SaveChangesAsync(cancellationToken);

            result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Successful,
                "Parking space was inserted successfully!");
        }
        catch (Exception e)
        {
            result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}