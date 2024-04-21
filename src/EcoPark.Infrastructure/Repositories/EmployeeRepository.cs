using EcoPark.Application.Employees.Delete;
using EcoPark.Application.Employees.Get;
using EcoPark.Application.Employees.Insert;
using EcoPark.Application.Employees.List;
using EcoPark.Application.Employees.Update;

namespace EcoPark.Infrastructure.Repositories;

public class EmployeeRepository(DatabaseDbContext databaseDbContext, IAuthenticationService authenticationService) : IRepository<EmployeeModel>
{
    public IUnitOfWork UnitOfWork { get; }

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
            .FirstOrDefaultAsync(e => e.Id == parsedCommand.Id, cancellationToken);

        if (employeeModel == null) return false;

        databaseDbContext.Employees.Remove(employeeModel);
        return true;
    }

    public async Task<EmployeeModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as GetEmployeeQuery;

        return await databaseDbContext.Employees.FirstOrDefaultAsync(e => e.Id == parsedQuery.EmployeeId,
            cancellationToken);
    }

    public async Task<IEnumerable<EmployeeModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListEmployeesQuery;

        bool hasEmployeeIds = parsedQuery.EmployeeIds != null && parsedQuery.EmployeeIds.Any();

        IQueryable<EmployeeModel> databaseQuery = databaseDbContext.Employees.AsNoTracking().AsQueryable();

        if (hasEmployeeIds)
            databaseQuery = databaseQuery.Where(e => parsedQuery.EmployeeIds!.Contains(e.Id));

        return await databaseQuery.ToListAsync(cancellationToken);
    }
}