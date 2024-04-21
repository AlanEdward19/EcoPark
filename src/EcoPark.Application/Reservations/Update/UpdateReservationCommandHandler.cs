﻿namespace EcoPark.Application.Reservations.Update;

public class UpdateReservationCommandHandler(IRepository<ReservationModel> repository) : IHandler<UpdateReservationCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateReservationCommand command, 
        CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            await repository.UnitOfWork.StartAsync(cancellationToken);

            var databaseOperationResult = await repository.UpdateAsync(command, cancellationToken);

            if (databaseOperationResult)
            {
                await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                await repository.UnitOfWork.CommitAsync(cancellationToken);

                result = new("Patch", EOperationStatus.Successful, "Reservation updated successfully");
            }
            else
            {
                await repository.UnitOfWork.RollbackAsync(cancellationToken);
                result = new("Patch", EOperationStatus.Failed, "No Reservations were found with this id");
            }
        }
        catch (Exception e)
        {
            await repository.UnitOfWork.RollbackAsync(cancellationToken);
            result = new("Patch", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}