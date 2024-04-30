namespace EcoPark.Application.Reservations.Update;

public class UpdateReservationStatusCommandHandler(IRepository<ReservationModel> repository) : IHandler<UpdateReservationStatusCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateReservationStatusCommand command, CancellationToken cancellationToken)
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

                    result = new("Patch", EOperationStatus.Successful, "Reservation status updated successfully");
                }
                else
                {
                    await repository.UnitOfWork.RollbackAsync(cancellationToken);
                    result = new("Patch", EOperationStatus.Failed, "No Reservations were found with this id");
                }
            }
            else
                result = new("Patch", EOperationStatus.NotAuthorized, "You have no permission to update this reservation status");
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new("Patch", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}
