using EcoPark.Domain.Interfaces.Database;

namespace EcoPark.Application.Rewards.Insert;

public class InsertRewardCommandHandler(IRepository<RewardModel> repository) : IHandler<InsertRewardCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertRewardCommand command, CancellationToken cancellationToken)
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
                        "Reservation was inserted successfully!");
                }
                else
                {
                    await repository.UnitOfWork.RollbackAsync(cancellationToken);
                    result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, "Reward was not inserted!");
                }
            }
            else
                result = new("Post", EOperationStatus.NotAuthorized, "You have no permission to create a Reward with this location");

        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new DatabaseOperationResponseViewModel("Post", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}