using Application;
using Infrastructure;

namespace EcoPark.Configurations;

public static class IoC
{
    public static IServiceCollection ConfigureIoC(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureInfrastructure(configuration)
            .ConfigureApplication();

        return services;
    }
}