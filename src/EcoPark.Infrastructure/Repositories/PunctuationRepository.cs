using EcoPark.Application.Punctuations;
using EcoPark.Application.Punctuations.Get;
using EcoPark.Application.Punctuations.List;

namespace EcoPark.Infrastructure.Repositories
{
    public class PunctuationRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork) : IRepository<PunctuationModel>
    {
        public IUnitOfWork UnitOfWork { get; } = unitOfWork;

        public async Task<EOperationStatus> CheckChangePermissionAsync(ICommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(ICommand command, CancellationToken cancellationToken)
        {
            var parsedCommand = command as PunctuationCommand;

            PunctuationModel punctuation =
                new(parsedCommand.ClientId, parsedCommand.LocationId, parsedCommand.Punctuation);

            await databaseDbContext.Punctuations.AddAsync(punctuation, cancellationToken);
        }

        public async Task UpdateAsync(ICommand command, CancellationToken cancellationToken)
        {
            var parsedCommand = command as PunctuationCommand;

            PunctuationModel punctuation = await databaseDbContext.Punctuations
                .FirstAsync(
                    p => p.ClientId == parsedCommand.ClientId && p.LocationId == parsedCommand.LocationId,
                    cancellationToken);

            punctuation.Punctuation += parsedCommand.Punctuation;

            databaseDbContext.Punctuations.Update(punctuation);
        }

        public async Task DeleteAsync(ICommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<PunctuationModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
        {
            var parsedQuery = query as GetPunctuationQuery;

            ClientModel? client = await databaseDbContext.Clients
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.Credentials)
                .FirstOrDefaultAsync(c => c.Credentials.Email == parsedQuery.RequestUserInfo.Email, cancellationToken);

            if (client == null) return null;

            return await databaseDbContext.Punctuations
                .AsNoTracking()
                .AsSplitQuery()
                .Include(punctuationModel => punctuationModel.Location)
                .FirstOrDefaultAsync(
                    p => p.ClientId == client.Id && p.LocationId == parsedQuery.LocationId,
                    cancellationToken);
        }

        public async Task<IEnumerable<PunctuationModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
        {
            var parsedQuery = query as ListPunctuationsQuery;

            ClientModel? client = await databaseDbContext.Clients
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.Credentials)
                .FirstOrDefaultAsync(c => c.Credentials.Email == parsedQuery.RequestUserInfo.Email, cancellationToken);

            if(client == null) return Enumerable.Empty<PunctuationModel>();

            IQueryable<PunctuationModel> databaseQuery = databaseDbContext.Punctuations
                .AsQueryable()
                .Include(x => x.Location);

            if (parsedQuery.LocationIds != null && parsedQuery.LocationIds.Any())
                databaseQuery = databaseQuery
                    .Where(p => parsedQuery.LocationIds.Contains(p.LocationId));

            databaseQuery = databaseQuery
                .Where(p => p.ClientId == client.Id);
           
            return await databaseQuery.ToListAsync(cancellationToken);
        }
    }
}
