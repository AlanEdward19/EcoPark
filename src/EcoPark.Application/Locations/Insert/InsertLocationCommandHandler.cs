namespace EcoPark.Application.Locations.Insert;

public class InsertLocationCommandHandler(DatabaseDbContext databaseContext) : IHandler<InsertLocationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertLocationCommand command, 
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;
        try
        {
            LocationModel locationModel = new(command.Name, command.Address);

            await databaseContext.Locations.AddAsync(locationModel, cancellationToken);

            await databaseContext.SaveChangesAsync(cancellationToken);

            result = new DatabaseOperationResponseViewModel("Post" ,EOperationStatus.Successful, "Location was inserted successfully!");
        }
        catch (Exception e)
        {
            result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}