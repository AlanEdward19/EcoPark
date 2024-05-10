using EcoPark.Domain.Interfaces.Database;

namespace EcoPark.Application.Rewards.Update.UseReward;

public class UseRewardCommandHandler(IRepository<ClientClaimedRewardModel> repository) : IHandler<UseRewardCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UseRewardCommand command, CancellationToken cancellationToken)
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

                    result = new("Patch", EOperationStatus.Successful, "Reward used successfully");
                }
                else
                {
                    await repository.UnitOfWork.RollbackAsync(cancellationToken);
                    result = new("Patch", EOperationStatus.Failed, "No Reward were found with this id");
                }
            }
            else
                result = new("Patch", EOperationStatus.NotAuthorized, "You have no permission to use this Reward");
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new("Patch", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}