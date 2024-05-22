using System.Text.Json;
using System.Text.Json.Serialization;
using EcoPark.Application.Employees.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace EcoPark.Presentation.Configurations;

public static class Controller
{
    public static IServiceCollection ConfigureController(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<InsertEmployeeCommandValidator>();
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

        return services;
    }
}