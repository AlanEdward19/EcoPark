using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DatabaseDbContext : DbContext
{
    public DatabaseDbContext(DbContextOptions<DatabaseDbContext> options) : base(options) { }
}