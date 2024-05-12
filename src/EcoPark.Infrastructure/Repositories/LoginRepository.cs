namespace EcoPark.Infrastructure.Repositories;

public class LoginRepository(DatabaseDbContext databaseDbContext, IAuthenticationService authenticationService, IUnitOfWork unitOfWork) : IRepository<CredentialsModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<EOperationStatus> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<CredentialsModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as LoginQuery;

        string hashedPassword = authenticationService.ComputeSha256Hash(parsedQuery.Password);

        return await databaseDbContext.Credentials
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == parsedQuery.Email.ToLower() && c.Password == hashedPassword, cancellationToken);
    }

    public async Task<IEnumerable<CredentialsModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}