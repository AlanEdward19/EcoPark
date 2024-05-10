namespace EcoPark.Application.Rewards.Insert.RedeemReward;

public class RedeemRewardCommandHandler(IRepository<ClientClaimedRewardModel> repository) : IHandler<RedeemRewardCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(RedeemRewardCommand command, CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            if (await repository.CheckChangePermissionAsync(command, cancellationToken))
            {
                await repository.UnitOfWork.StartAsync(cancellationToken);

                var databaseOperationResult = await repository.AddAsync(command, cancellationToken);

                if (databaseOperationResult)
                {
                    await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                    await repository.UnitOfWork.CommitAsync(cancellationToken);

                    result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Successful,
                        "Reward was inserted successfully!");
                }
                else
                {
                    await repository.UnitOfWork.RollbackAsync(cancellationToken);
                    result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, "Reward was not redeemed!");
                }
            }
            else
                result = new("Post", EOperationStatus.NotAuthorized, "You have no permission to redeem a Reward with this location");

        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}