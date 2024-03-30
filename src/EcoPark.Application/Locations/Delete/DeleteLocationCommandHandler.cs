namespace EcoPark.Application.Locations.Delete;

public class DeleteLocationCommandHandler(DatabaseDbContext databaseDbContext) : IHandler<DeleteLocationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(DeleteLocationCommand command,
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            LocationModel? locationModel = await databaseDbContext.Locations
                .FirstOrDefaultAsync(l => l.Id == command.Id, cancellationToken);

            if (locationModel != null)
            {
                databaseDbContext.Locations.Remove(locationModel);

                await databaseDbContext.SaveChangesAsync(cancellationToken);

                result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Successful,
                    "Location was deleted successfully!");
            }
            else
                result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed,
                    "No Location were found with this id");
        }
        catch (Exception e)
        {
            result = new DatabaseOperationResponseViewModel("Delete", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}