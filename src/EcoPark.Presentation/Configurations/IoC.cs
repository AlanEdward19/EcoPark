using EcoPark.Application;
using EcoPark.Infrastructure;

namespace EcoPark.Presentation.Configurations;

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