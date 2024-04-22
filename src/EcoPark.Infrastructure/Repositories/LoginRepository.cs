namespace EcoPark.Infrastructure.Repositories;

public class LoginRepository(DatabaseDbContext databaseDbContext, IAuthenticationService authenticationService, IUnitOfWork unitOfWork) : IRepository<UserModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<UserModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as LoginQuery;

        string hashedPassword = authenticationService.ComputeSha256Hash(parsedQuery.Password);

        if (parsedQuery.IsEmployee)
            return await databaseDbContext.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Email == parsedQuery.Email && e.Password == hashedPassword,
                    cancellationToken);

        return await databaseDbContext.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == parsedQuery.Email && c.Password == hashedPassword, cancellationToken);
    }

    public async Task<IEnumerable<UserModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}