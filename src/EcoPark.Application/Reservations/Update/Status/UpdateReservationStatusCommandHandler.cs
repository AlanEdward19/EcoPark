namespace EcoPark.Application.Reservations.Update.Status;

public class UpdateReservationStatusCommandHandler(IRepository<ReservationModel> repository) : IHandler<UpdateReservationStatusCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateReservationStatusCommand command, CancellationToken cancellationToken)
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

                    result = new(EOperationStatus.Successful, "Reservation status updated successfully");

                    break;

                case EOperationStatus.NotAuthorized:
                    result = new(EOperationStatus.NotAuthorized, "You have no permission to update this reservation status");
                    break;

                case EOperationStatus.Failed:
                    break;

                case EOperationStatus.NotFound:
                    result = new(EOperationStatus.NotFound, "Reservation not found");
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
