namespace EcoPark.Application.Locations.Update;

public class UpdateLocationCommandHandler(DatabaseDbContext databaseDbContext) : IHandler<UpdateLocationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateLocationCommand command, 
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            LocationModel? locationModel = await databaseDbContext.Locations
                .FirstOrDefaultAsync(l => l.Id == command.LocationId, cancellationToken);

            if (locationModel != null)
            {
                LocationAggregateRoot locationAggregate = new(locationModel);

                locationAggregate.UpdateName(command.Name);
                locationAggregate.UpdateAddress(command.Address);

                locationModel.UpdateBasedOnAggregate(locationAggregate);

                databaseDbContext.Locations.Update(locationModel);

                await databaseDbContext.SaveChangesAsync(cancellationToken);

                result = new("Patch", EOperationStatus.Successful, "Location updated successfully");
            }
            else
                result = new("Patch", EOperationStatus.Failed, "No Location were found with this id");
        }
        catch (Exception e)
        {
            result = new("Patch", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}