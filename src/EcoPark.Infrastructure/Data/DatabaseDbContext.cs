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

        modelBuilder.Entity<CarModel>()
            .HasOne(c => c.Client)
            .WithMany(c => c.Cars)
            .HasForeignKey(c => c.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClientModel>()
            .HasOne(c => c.Reservation)
            .WithOne(r => r.Client)
            .HasForeignKey<ReservationModel>(r => r.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ReservationModel>()
            .HasOne(r => r.Car)
            .WithOne(c => c.Reservation)
            .OnDelete(DeleteBehavior.Restrict);
    }

}