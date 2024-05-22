using EcoPark.Domain.Interfaces.Database;

namespace EcoPark.Application.Rewards.Update.UseReward;

public class UseRewardCommandHandler(IRepository<ClientClaimedRewardModel> repository) : IHandler<UseRewardCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UseRewardCommand command, CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result = new(EOperationStatus.Failed, "AAAAA");

        try
        {
            EOperationStatus status = await repository.CheckChangePermissionAsync(command, cancellationToken);

            switch (status)
            {
                case EOperationStatus.Successful:
                    await repository.UnitOfWork.StartAsync(cancellationToken);

                    await repository.UpdateAsync(command, cancellationToken);

                    await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    await repository.UnitOfWork.CommitAsync(cancellationToken);

                    result = new(EOperationStatus.Successful, "Reward used successfully");

                    break;

                case EOperationStatus.NotAuthorized:
                    result = new(EOperationStatus.NotAuthorized, "You have no permission to use this Reward");
                    break;

                case EOperationStatus.Failed:
                    result = new(EOperationStatus.Failed, "Invalid operation, client wasn't found");
                    break;

                case EOperationStatus.NotFound:
                    result = new(EOperationStatus.NotFound, "Reward not found");
                    break;
            }
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new(EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}