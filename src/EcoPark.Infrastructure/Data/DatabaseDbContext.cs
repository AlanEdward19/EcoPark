using Microsoft.EntityFrameworkCore;

namespace EcoPark.Infrastructure.Data;

public partial class DatabaseDbContext : DbContext
{
    public DatabaseDbContext(DbContextOptions<DatabaseDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeModel>()
            .HasMany(e => e.GroupAccesses)
            .WithOne(ga => ga.Employee)
            .HasForeignKey(ga => ga.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GroupAccessModel>()
            .HasOne(ga => ga.Employee)
            .WithMany(e => e.GroupAccesses)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PunctuationModel>()
            .HasOne(p => p.Client)
            .WithMany(c => c.Punctuations)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.NoAction);
    }

}