using System.Text.Json.Serialization;

namespace EcoPark.Presentation.Configurations;

public static class Controller
{
    public static IServiceCollection ConfigureController(this IServiceCollection services)
    {
        //services
        //    .AddControllers()
        //    .AddFluentValidation(c =>
        //    {
        //        c.RegisterValidatorsFromAssemblyContaining<CreateInstructorCommandValidator>();
        //        c.ValidatorOptions.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        //    });

        services.AddControllersWithViews()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        return services;
    }
}