﻿namespace EcoPark.Application.Employees.Insert.System;

public class InsertSystemCommandHandler(IRepository<EmployeeModel> repository) : IHandler<InsertSystemCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(InsertSystemCommand command, CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result = new(EOperationStatus.Failed, "AAAAA");

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

                    result = new DatabaseOperationResponseViewModel(EOperationStatus.Successful, "System was inserted successfully!");

                    break;

                case EOperationStatus.NotAuthorized:
                    result = new DatabaseOperationResponseViewModel(EOperationStatus.NotAuthorized, "You don't have permission to insert an employee with this UserType!");
                    break;

                case EOperationStatus.Failed:
                    result = new DatabaseOperationResponseViewModel(EOperationStatus.Failed, "E-mail is not available");
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