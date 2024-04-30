using Microsoft.EntityFrameworkCore;

namespace EcoPark.Infrastructure.Data;

public partial class DatabaseDbContext : DbContext
{
    public DatabaseDbContext(DbContextOptions<DatabaseDbContext> options) : base(options) { }
}