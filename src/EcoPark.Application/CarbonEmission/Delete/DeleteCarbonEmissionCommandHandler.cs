namespace EcoPark.Application.CarbonEmission.Delete;

public class DeleteCarbonEmissionCommandHandler(IRepository<CarbonEmissionModel> repository) : IHandler<DeleteCarbonEmissionCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(DeleteCarbonEmissionCommand command, CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result = new(EOperationStatus.Failed, "");

        try
        {
            EOperationStatus status = await repository.CheckChangePermissionAsync(command, cancellationToken);

            switch (status)
            {
                case EOperationStatus.Successful:
                    await repository.UnitOfWork.StartAsync(cancellationToken);

                    await repository.DeleteAsync(command, cancellationToken);

                    await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    await repository.UnitOfWork.CommitAsync(cancellationToken);

                    result = new DatabaseOperationResponseViewModel(EOperationStatus.Successful,
                        "CarbonEmission was deleted successfully!");
                    break;

                case EOperationStatus.NotAuthorized:
                    result = new(EOperationStatus.NotAuthorized, "You have no permission to delete this carbonEmission");
                    break;

                case EOperationStatus.Failed:
                    result = new DatabaseOperationResponseViewModel(EOperationStatus.Failed, "No system were found with this email, contact administrator");
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