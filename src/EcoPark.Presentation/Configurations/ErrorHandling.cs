using EcoPark.Presentation.Middlewares;

namespace EcoPark.Presentation.Configurations;

public static class ErrorHandling
{
    public static IApplicationBuilder ConfigureMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}