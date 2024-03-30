namespace EcoPark.Application.ParkingSpaces.Update;

public class UpdateParkingSpaceCommandHandler(DatabaseDbContext databaseDbContext) : IHandler<UpdateParkingSpaceCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateParkingSpaceCommand command,
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            ParkingSpaceModel? parkingSpaceModel = await databaseDbContext.ParkingSpaces
                .FirstOrDefaultAsync(p => p.Id == command.ParkingSpaceId, cancellationToken);

            if (parkingSpaceModel != null)
            {
                ParkingSpaceAggregate parkingSpaceAggregate = new(parkingSpaceModel);

                parkingSpaceAggregate.UpdateFloor(command.Floor);
                parkingSpaceAggregate.UpdateParkingSpaceName(command.ParkingSpaceName);
                parkingSpaceAggregate.UpdateParkingSpaceType(command.ParkingSpaceType);
                parkingSpaceAggregate.SetOccupied(command.IsOccupied);

                parkingSpaceModel.UpdateBasedOnAggregate(parkingSpaceAggregate);

                databaseDbContext.ParkingSpaces.Update(parkingSpaceModel);

                await databaseDbContext.SaveChangesAsync(cancellationToken);

                result = new("Patch", EOperationStatus.Successful, "Parking space updated successfully");
            }
            else
                result = new("Patch", EOperationStatus.Failed, "No Parking space were found with this id");
        }
        catch (Exception e)
        {
            result = new("Patch", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}