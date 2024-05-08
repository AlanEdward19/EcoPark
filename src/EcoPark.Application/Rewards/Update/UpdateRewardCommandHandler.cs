namespace EcoPark.Application.Rewards.Update;

public class UpdateRewardCommandHandler(IRepository<RewardModel> repository) : IHandler<UpdateRewardCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateRewardCommand command,
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            if (await repository.CheckChangePermissionAsync(command, cancellationToken))
            {
                await repository.UnitOfWork.StartAsync(cancellationToken);

                var databaseOperationResult = await repository.UpdateAsync(command, cancellationToken);

                if (databaseOperationResult)
                {
                    await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    await repository.UnitOfWork.CommitAsync(cancellationToken);

                    result = new("Patch", EOperationStatus.Successful, "Reward updated successfully");
                }
                else
                {
                    await repository.UnitOfWork.RollbackAsync(cancellationToken);
                    result = new("Patch", EOperationStatus.Failed, "No Reward were found with this id");
                }
            }
            else
                result = new("Patch", EOperationStatus.NotAuthorized, "You have no permission to update this Reward");
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new("Patch", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}