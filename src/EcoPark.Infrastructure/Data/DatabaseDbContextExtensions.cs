using Microsoft.EntityFrameworkCore;

namespace EcoPark.Infrastructure.Data;

public static class DatabaseDbContextExtensions
{
    public static void AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DatabaseDbContext>(options =>
            options.UseSqlServer(connectionString));
    }
}