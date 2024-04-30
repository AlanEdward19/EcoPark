using EcoPark.Application.Employees.Delete;
using EcoPark.Application.Employees.Get;
using EcoPark.Application.Employees.Insert;
using EcoPark.Application.Employees.List;
using EcoPark.Application.Employees.Update;
using EcoPark.Domain.Commons.Enums;

namespace EcoPark.Infrastructure.Repositories;

public class EmployeeRepository(DatabaseDbContext databaseDbContext, IAuthenticationService authenticationService, IUnitOfWork unitOfWork) : IRepository<EmployeeModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        EmployeeModel? employeeModel = null;
        EmployeeModel? administratorModel = null;
        var requesterUserType = command.RequestUserInfo.UserType;

        switch (command)
        {
            case InsertEmployeeCommand insertCommand:

                if (requesterUserType != EUserType.PlataformAdministrator)
                    return (int)requesterUserType >= (int)insertCommand.UserType!.Value;

                return true;

            case UpdateEmployeeCommand updateCommand:
                employeeModel = await databaseDbContext.Employees
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(updateCommand.RequestUserInfo.Email)
                                              &&
                                              e.Id == updateCommand.EmployeeId, cancellationToken);

                if (employeeModel == null)
                {
                    administratorModel = await databaseDbContext.Employees
                        .AsNoTracking()
                        .Include(x => x.Credentials)
                        .Include(x => x.Employees)
                        .FirstOrDefaultAsync(x =>
                            x.Credentials.Email.Equals(command.RequestUserInfo.Email) &&
                            x.Credentials.UserType == EUserType.Administrator, cancellationToken);

                    if (administratorModel != null)
                        employeeModel =
                            administratorModel.Employees.FirstOrDefault(x => x.Id == updateCommand.EmployeeId);
                }
                break;

            case DeleteEmployeeCommand deleteCommand:
                administratorModel = await databaseDbContext.Employees
                    .AsNoTracking()
                    .Include(x => x.Credentials)
                    .Include(x => x.Employees)
                    .FirstOrDefaultAsync(x =>
                        x.Credentials.Email.Equals(command.RequestUserInfo.Email) &&
                        x.Credentials.UserType == EUserType.Administrator, cancellationToken);

                if (administratorModel != null)
                    employeeModel = administratorModel.Employees.FirstOrDefault(x => x.Id == deleteCommand.Id);

                break;
        }

        return employeeModel != null;
    }

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertEmployeeCommand;

        EmployeeModel employeeModel = parsedCommand.ToModel(authenticationService);

        await databaseDbContext.Employees.AddAsync(employeeModel, cancellationToken);

        return true;
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UpdateEmployeeCommand;

        EmployeeModel? employeeModel = await databaseDbContext.Employees
            .Include(x => x.Credentials)
            .FirstOrDefaultAsync(e => e.Id == parsedCommand.EmployeeId, cancellationToken);

        if (employeeModel != null)
        {
            EmployeeValueObject employeeValueObject = new(employeeModel);

            employeeValueObject.UpdateEmail(parsedCommand.Email);
            employeeValueObject.UpdatePassword(authenticationService.ComputeSha256Hash(parsedCommand.Password!));
            employeeValueObject.UpdateFirstName(parsedCommand.FirstName);
            employeeValueObject.UpdateLastName(parsedCommand.LastName);
            employeeValueObject.UpdateUserType(parsedCommand.UserType);

            employeeModel.UpdateBasedOnValueObject(employeeValueObject);

            databaseDbContext.Employees.Update(employeeModel);

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as DeleteEmployeeCommand;

        EmployeeModel? employeeModel = await databaseDbContext.Employees
            .Include(x => x.Credentials)
            .FirstOrDefaultAsync(e => e.Id == parsedCommand.Id, cancellationToken);

        if (employeeModel == null) return false;

        databaseDbContext.Employees.Remove(employeeModel);
        databaseDbContext.Credentials.Remove(employeeModel.Credentials);
        return true;
    }

    public async Task<EmployeeModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as GetEmployeeQuery;

        EmployeeModel? administratorModel = await databaseDbContext.Employees
            .AsQueryable()
            .Include(x => x.Credentials)
            .Include(x => x.Employees)
            .FirstOrDefaultAsync(
                e => e.Credentials.Email.Equals(query.RequestUserInfo.Email) &&
                     e.Credentials.UserType == EUserType.Administrator, cancellationToken);

        if (administratorModel == null) return null;

        return administratorModel.Employees.FirstOrDefault(x => x.Id == parsedQuery.EmployeeId);
    }

    public async Task<IEnumerable<EmployeeModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListEmployeesQuery;

        bool hasEmployeeIds = parsedQuery.EmployeeIds != null && parsedQuery.EmployeeIds.Any();

        EmployeeModel? administratorModel = await databaseDbContext.Employees
            .AsQueryable()
            .Include(x => x.Credentials)
            .FirstOrDefaultAsync(
                e => e.Credentials.Email.Equals(query.RequestUserInfo.Email) &&
                     e.Credentials.UserType == EUserType.Administrator, cancellationToken);

        if (administratorModel == null) return Enumerable.Empty<EmployeeModel>();

        IQueryable<EmployeeModel> databaseQuery =
            databaseDbContext.Employees
                .AsNoTracking()
                .Include(x => x.Credentials)
                .AsQueryable()
                .Where(x => x.AdministratorId.Equals(administratorModel.Id));

        if (hasEmployeeIds)
            databaseQuery = databaseQuery.Where(e => parsedQuery.EmployeeIds!.Contains(e.Id));

        return await databaseQuery.ToListAsync(cancellationToken);
    }
}