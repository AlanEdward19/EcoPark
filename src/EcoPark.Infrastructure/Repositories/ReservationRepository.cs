using EcoPark.Application.Reservations.Delete;
using EcoPark.Application.Reservations.Get;
using EcoPark.Application.Reservations.Insert;
using EcoPark.Application.Reservations.List;
using EcoPark.Application.Reservations.Update;

namespace EcoPark.Infrastructure.Repositories;

public class ReservationRepository(DatabaseDbContext databaseDbContext, IUnitOfWork unitOfWork) : IRepository<ReservationModel>
{
    public IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public async Task<bool> AddAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as InsertReservationCommand;

        throw new NotImplementedException("Feature em desenvolvimento");
    }

    public async Task<bool> UpdateAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as UpdateReservationCommand;

        throw new NotImplementedException("Feature em desenvolvimento");
    }

    public async Task<bool> DeleteAsync(ICommand command, CancellationToken cancellationToken)
    {
        var parsedCommand = command as DeleteReservationCommand;

        throw new NotImplementedException("Feature em desenvolvimento");
    }

    public async Task<ReservationModel?> GetByIdAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as GetReservationQuery;

        IQueryable<ReservationModel> reservationQuery = databaseDbContext.Reservations.AsNoTracking().AsQueryable();

        if (parsedQuery.IncludeParkingSpace)
            reservationQuery.Include(r => r.ParkingSpace);

        return await reservationQuery.FirstOrDefaultAsync(r => r.Id == parsedQuery.ReservationId, cancellationToken);
    }

    public async Task<IEnumerable<ReservationModel>> ListAsync(IQuery query, CancellationToken cancellationToken)
    {
        var parsedQuery = query as ListReservationQuery;

        throw new NotImplementedException("Feature em desenvolvimento");
    }
}