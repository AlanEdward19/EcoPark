using EcoPark.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace EcoPark.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureDatabase(configuration)
            .ConfigureWebSocket()
            .ConfigureRepositories();

        return services;
    }

    private static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext(configuration.GetConnectionString("MainDatabase"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAggregateRepository<LocationModel>, LocationRepository>();
        services.AddScoped<IAggregateRepository<ParkingSpaceModel>, ParkingSpaceRepository>();
        services.AddScoped<IRepository<EmployeeModel>, EmployeeRepository>();
        services.AddScoped<IRepository<ReservationModel>, ReservationRepository>();
        services.AddScoped<IRepository<CredentialsModel>, LoginRepository>();
        services.AddScoped<IAggregateRepository<ClientModel>, ClientRepository>();
        services.AddScoped<IRepository<CarModel>, CarRepository>();

        return services;
    }

    private static IServiceCollection ConfigureWebSocket(this IServiceCollection services)
    {
        services
            .AddSignalR();

        return services;
    }

    public static IApplicationBuilder UpdateMigrations(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

        var context = serviceScope?.ServiceProvider.GetRequiredService<DatabaseDbContext>();

        if (context != null)
        {
            try
            {
                var pendingMigrations = context.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        return app;
    }
}