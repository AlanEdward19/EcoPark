namespace EcoPark.Application.Locations.Insert;

public class InsertLocationCommandHandler(IRepository<LocationModel> repository) : IHandler<InsertLocationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertLocationCommand command,
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

                    result = new DatabaseOperationResponseViewModel(EOperationStatus.Successful, "Location was inserted successfully!");

                    break;

                case EOperationStatus.NotAuthorized:
                    result = new DatabaseOperationResponseViewModel(EOperationStatus.NotAuthorized, "You have no permission to insert this location");
                    break;

                case EOperationStatus.Failed:
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