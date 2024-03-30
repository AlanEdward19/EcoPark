using Microsoft.EntityFrameworkCore;

namespace EcoPark.Infrastructure.Data;

public partial class DatabaseDbContext : DbContext
{
    public DatabaseDbContext(DbContextOptions<DatabaseDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParkingSpaceModel>()
            .HasMany(p => p.Reservations)
            .WithOne(r => r.ParkingSpace)
            .HasForeignKey(r => r.ParkingSpaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}