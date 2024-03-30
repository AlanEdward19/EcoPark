using EcoPark.Infrastructure.Data;
using Microsoft.Extensions.Configuration;

namespace EcoPark.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureDatabase(configuration);

        return services;
    }

    private static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext(configuration.GetConnectionString("MainDatabase"));

        return services;
    }
}