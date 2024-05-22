namespace EcoPark.Application.CarbonEmission.Insert;

public class InsertCarbonEmissionCommandHandler(IRepository<CarbonEmissionModel> repository) : IHandler<InsertCarbonEmissionCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertCarbonEmissionCommand command, CancellationToken cancellationToken)
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

                    result = new DatabaseOperationResponseViewModel(EOperationStatus.Successful, "CarbonEmission was inserted successfully!");
                    break;

                case EOperationStatus.NotAuthorized:
                    result = new(EOperationStatus.NotAuthorized, "You are not authorized to insert a carbon emission for this reservation");
                    break;

                case EOperationStatus.Failed:
                    result = new(EOperationStatus.Failed, "No client were found with this email, contact administrator");
                    break;

                case EOperationStatus.NotFound:
                    result = new(EOperationStatus.NotFound, "No reservation were found with this id");
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