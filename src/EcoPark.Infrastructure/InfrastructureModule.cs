using EcoPark.Domain.Interfaces.Providers;
using EcoPark.Infrastructure.Providers;
using EcoPark.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EcoPark.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureDatabase(configuration)
            .ConfigureWebSocket()
            .AddProviders(configuration)
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
        services.AddScoped<IRepository<LocationModel>, LocationRepository>();
        services.AddScoped<IRepository<ParkingSpaceModel>, ParkingSpaceRepository>();
        services.AddScoped<IRepository<EmployeeModel>, EmployeeRepository>();
        services.AddScoped<IRepository<ReservationModel>, ReservationRepository>();
        services.AddScoped<IRepository<CredentialsModel>, LoginRepository>();
        services.AddScoped<IRepository<ClientModel>, ClientRepository>();
        services.AddScoped<IRepository<CarModel>, CarRepository>();
        services.AddScoped<IRepository<PunctuationModel>, PunctuationRepository>();
        services.AddScoped<IRepository<RewardModel>, RewardRepository>();
        services.AddScoped<IRepository<ClientClaimedRewardModel>, ClientClaimedRewardRepository>();
        services.AddScoped<IRepository<CarbonEmissionModel>, CarbonEmissionRepository>();

        return services;
    }

    private static IServiceCollection ConfigureWebSocket(this IServiceCollection services)
    {
        services
            .AddSignalR();

        return services;
    }

    private static IServiceCollection AddProviders(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IStorageProvider>(_ =>
            new StorageProvider(configuration.GetConnectionString("StorageAccount"), new Logger<StorageProvider>(new LoggerFactory())));

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