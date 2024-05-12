using EcoPark.Application.Employees.Delete;
using EcoPark.Application.Employees.Get;
using EcoPark.Application.Employees.Insert;
using EcoPark.Application.Employees.Insert.GroupAccess;
using EcoPark.Application.Employees.List;
using EcoPark.Application.Employees.Update;
using EcoPark.Application.Employees.Delete.GroupAccess;
using EcoPark.Application.Employees.Insert.System;
using EcoPark.Domain.Interfaces.Providers;

namespace EcoPark.Infrastructure.Repositories;

public class EmployeeRepository(DatabaseDbContext databaseDbContext, IAuthenticationService authenticationService, IUnitOfWork unitOfWork, IStorageProvider storageProvider) : IRepository<EmployeeModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<EOperationStatus> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        EmployeeModel? employeeModel = null;
        EmployeeModel? administratorModel = null;
        LocationModel? locationModel = null;
        var requesterUserType = command.RequestUserInfo.UserType;

        switch (command)
        {
            case InsertSystemCommand:
                if(requesterUserType is EUserType.PlataformAdministrator or EUserType.Administrator)
                    return EOperationStatus.Successful;

                break;

            case InsertEmployeeGroupAccessCommand insertEmployeeGroupAccessCommand:

                administratorModel = await databaseDbContext.Employees
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(command.RequestUserInfo.Email),
                        cancellationToken);

                if (administratorModel == null) return EOperationStatus.NotAuthorized;

                employeeModel = await databaseDbContext.Employees
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(e => e.Id == insertEmployeeGroupAccessCommand.EmployeeId, cancellationToken);

                if (employeeModel == null) return EOperationStatus.NotFound;

                locationModel = await databaseDbContext.Locations
                    .FirstOrDefaultAsync(x => x.Id == insertEmployeeGroupAccessCommand.LocationId, cancellationToken);

                if (locationModel == null) return EOperationStatus.NotFound;

                bool canGivePermission = administratorModel.Credentials.UserType == EUserType.Administrator &&
                                         (administratorModel.AdministratorId == employeeModel.AdministratorId ||
                                          administratorModel.Id == employeeModel.AdministratorId) &&
                                         (locationModel.OwnerId == administratorModel.Id ||
                                          locationModel.OwnerId == administratorModel.AdministratorId);

                return canGivePermission ? EOperationStatus.Successful : EOperationStatus.NotAuthorized;

            case InsertEmployeeCommand insertCommand:

                if (requesterUserType != EUserType.PlataformAdministrator)
                    return (int)requesterUserType >= (int)insertCommand.UserType!.Value ? EOperationStatus.Successful : EOperationStatus.NotAuthorized;

                return EOperationStatus.Successful;

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

                    else if (administratorModel == null && employeeModel == null)
                        return EOperationStatus.NotFound;

                    else
                        return EOperationStatus.NotAuthorized;
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

                else
                    return EOperationStatus.Failed;

                break;

            case DeleteEmployeeGroupAccessCommand deleteEmployeeGroupAccessCommand:

                administratorModel = await databaseDbContext.Employees
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(e => e.Credentials.Email.Equals(command.RequestUserInfo.Email), cancellationToken);

                if (administratorModel == null) return EOperationStatus.NotAuthorized;

                employeeModel = await databaseDbContext.Employees
                    .Include(x => x.Credentials)
                    .FirstOrDefaultAsync(e => e.Id == deleteEmployeeGroupAccessCommand.EmployeeId, cancellationToken);

                if (employeeModel == null) return EOperationStatus.NotFound;

                locationModel = await databaseDbContext.Locations
                    .FirstOrDefaultAsync(x => x.Id == deleteEmployeeGroupAccessCommand.LocationId, cancellationToken);

                if (locationModel == null) return EOperationStatus.NotFound;

                bool canRemovePermission = administratorModel.Credentials.UserType == EUserType.Administrator &&
                                           (administratorModel.AdministratorId == employeeModel.AdministratorId ||
                                            administratorModel.Id == employeeModel.AdministratorId) &&
                                           (locationModel.OwnerId == administratorModel.Id ||
                                            locationModel.OwnerId == administratorModel.AdministratorId);

                return canRemovePermission ? EOperationStatus.Successful : EOperationStatus.NotAuthorized;
        }

        return employeeModel != null ? EOperationStatus.Successful : EOperationStatus.NotAuthorized;
    }

    public async Task AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        switch (command)
        {
            case InsertEmployeeCommand insertCommand:
                await InsertEmployeeAsync(insertCommand, cancellationToken);
                break;

            case InsertEmployeeGroupAccessCommand insertEmployeeGroupAccessCommand:
                await InsertEmployeeGroupAccessAsync(insertEmployeeGroupAccessCommand, cancellationToken);
                break;

            case InsertSystemCommand insertSystemCommand:
                await InsertSystemAsync(insertSystemCommand, cancellationToken);
                break;
        }
    }

    public async Task UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UpdateEmployeeCommand;

        EmployeeModel employeeModel = await databaseDbContext.Employees
            .Include(x => x.Credentials)
            .FirstAsync(e => e.Id == parsedCommand.EmployeeId, cancellationToken);

        EmployeeValueObject employeeValueObject = new(employeeModel);

        employeeValueObject.UpdateEmail(parsedCommand.Email);
        employeeValueObject.UpdatePassword(authenticationService.ComputeSha256Hash(parsedCommand.Password!));
        employeeValueObject.UpdateFirstName(parsedCommand.FirstName);
        employeeValueObject.UpdateLastName(parsedCommand.LastName);
        employeeValueObject.UpdateUserType(parsedCommand.UserType);

        if (parsedCommand.Image != null)
        {
            string format = parsedCommand.ImageFileName!.Split('.').Last();
            string blobName;

            string newFileFormat = parsedCommand.ImageFileName!.Split('.').Last();

            if (string.IsNullOrWhiteSpace(employeeModel.Credentials.Image))
                blobName = $"{Guid.NewGuid()}.{format}";

            else
            {
                blobName = employeeModel.Credentials.Image;
                var oldFileFormat = employeeModel.Credentials.Image!.Split('.').Last();

                if (!newFileFormat.Equals(oldFileFormat))
                {
                    string oldFileName = employeeModel.Credentials.Image.Split(".").First();
                    blobName = $"{oldFileName}.{newFileFormat}";
                    employeeValueObject.UpdateImage(blobName);
                }

                await storageProvider.DeleteBlobAsync(employeeModel.Credentials.Image, "profiles");
            }

            await storageProvider.WriteBlobAsync(parsedCommand.Image, blobName, "profiles");
        }

        employeeModel.UpdateBasedOnValueObject(employeeValueObject);

        databaseDbContext.Employees.Update(employeeModel);
    }

    public async Task DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        switch (command)
        {
            case DeleteEmployeeCommand deleteEmployeeCommand:
                await DeleteEmployeeAsync(deleteEmployeeCommand, cancellationToken);
                break;

            case DeleteEmployeeGroupAccessCommand deleteEmployeeGroupAccessCommand:
                await DeleteEmployeeGroupAccessAsync(deleteEmployeeGroupAccessCommand, cancellationToken);
                break;

        }
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

    private async Task InsertEmployeeAsync(InsertEmployeeCommand command, CancellationToken cancellationToken)
    {
        string? blobFileName = null;

        if (command.Image != null)
        {
            Guid imageId = Guid.NewGuid();
            string format = command.ImageFileName.Split('.').Last();
            blobFileName = $"{imageId}.{format}";

            await storageProvider.WriteBlobAsync(command.Image, blobFileName, "profiles");
        }

        EmployeeModel employeeModel = command.ToModel(authenticationService, blobFileName, null);

        await databaseDbContext.Employees.AddAsync(employeeModel, cancellationToken);
    }

    private async Task InsertEmployeeGroupAccessAsync(InsertEmployeeGroupAccessCommand command, CancellationToken cancellationToken)
    {
        await databaseDbContext.GroupAccesses
            .AddAsync(new GroupAccessModel(command.LocationId, command.EmployeeId), cancellationToken);
    }

    private async Task InsertSystemAsync(InsertSystemCommand command, CancellationToken cancellationToken)
    {
        string email = command.RequestUserInfo.Email;

        EmployeeModel administratorModel = await databaseDbContext.Employees
            .Include(x => x.Credentials)
            .FirstAsync(e => e.Credentials.Email.Equals(email), cancellationToken);

        EmployeeModel employeeModel = command.ToModel(authenticationService, administratorModel.Id, command.Ipv4);

        await databaseDbContext.Employees.AddAsync(employeeModel, cancellationToken);
    }

    private async Task DeleteEmployeeAsync(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        EmployeeModel employeeModel = await databaseDbContext.Employees
            .Include(x => x.Credentials)
            .FirstAsync(e => e.Id == command.Id, cancellationToken);

        databaseDbContext.Employees.Remove(employeeModel);
        databaseDbContext.Credentials.Remove(employeeModel.Credentials);
    }

    private async Task DeleteEmployeeGroupAccessAsync(DeleteEmployeeGroupAccessCommand command, CancellationToken cancellationToken)
    {
        GroupAccessModel groupAccessModel = await databaseDbContext.GroupAccesses
            .FirstAsync(x => x.EmployeeId == command.EmployeeId && x.LocationId == command.LocationId,
                cancellationToken);

        databaseDbContext.GroupAccesses.Remove(groupAccessModel);
    }
}