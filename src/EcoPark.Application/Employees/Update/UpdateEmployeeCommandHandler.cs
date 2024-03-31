using EcoPark.Domain.ValueObjects;

namespace EcoPark.Application.Employees.Update;

public class UpdateEmployeeCommandHandler(DatabaseDbContext databaseDbContext, IAuthenticationService authenticationService) : IHandler<UpdateEmployeeCommand, DatabaseOperationResponseViewModel>
{
    public async Task<DatabaseOperationResponseViewModel> HandleAsync(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        DatabaseOperationResponseViewModel result;

        try
        {
            EmployeeModel? employeeModel = await databaseDbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == command.EmployeeId, cancellationToken);

            if (employeeModel != null)
            {
                EmployeeValueObject employeeValueObject = new(employeeModel);

                employeeValueObject.UpdateEmail(command.Email);
                employeeValueObject.UpdatePassword(authenticationService.ComputeSha256Hash(command.Password!));
                employeeValueObject.UpdateFirstName(command.FirstName);
                employeeValueObject.UpdateLastName(command.LastName);
                employeeValueObject.UpdateUserType(command.UserType);

                employeeModel.UpdateBasedOnValueObject(employeeValueObject);

                databaseDbContext.Employees.Update(employeeModel);

                await databaseDbContext.SaveChangesAsync(cancellationToken);

                result = new("Patch", EOperationStatus.Successful, "Employee updated successfully");
            }
            else
                result = new("Patch", EOperationStatus.Failed, "No Employee were found with this id");
        }
        catch (Exception e)
        {
            result = new("Patch", EOperationStatus.Failed, e.Message);
        }

        return result;
    }
}