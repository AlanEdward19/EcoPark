using EcoPark.Infrastructure.WebSocket;

namespace EcoPark.Presentation.Configurations;

public static class Endpoint
{
    public static IApplicationBuilder ConfigureEndpoints(this IApplicationBuilder app, IConfigurationSection section)
    {
        var apiHealthCheckUrl = section["APIHealthCheckUrl"];

        app
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks(apiHealthCheckUrl);
                endpoints.MapHub<ParkingSpaceHub>("/parkingSpaceHub");
            });

        return app;
    }
}