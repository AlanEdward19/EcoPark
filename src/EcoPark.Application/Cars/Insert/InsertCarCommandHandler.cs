namespace EcoPark.Application.Cars.Insert;

public class InsertCarCommandHandler(IRepository<CarModel> repository) : IHandler<InsertCarCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertCarCommand command,
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result = new(EOperationStatus.Failed, "");
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

                    result = new DatabaseOperationResponseViewModel(EOperationStatus.Successful, "Car was inserted successfully!");
                    break;

                case EOperationStatus.NotAuthorized:
                    break;

                case EOperationStatus.Failed:
                    result = new(EOperationStatus.Failed, "No client were found with this email, contact administrator");
                    break;

                case EOperationStatus.NotFound:
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