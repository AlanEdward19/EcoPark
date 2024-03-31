using EcoPark.Presentation.Configurations;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .ConfigureController()
    .ConfigureIoC(builder.Configuration)
    .ConfigureCors()
    .ConfigureAuthentication(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "EcoPark.API", Version = "v1"});

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header usando o esquema Bearer."
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    })
    .AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .UseCors("CorsPolicy")
    .ConfigureMiddleware()
    .UseHttpsRedirection()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .ConfigureEndpoints(builder.Configuration.GetSection("EndPointsConfig"));

app.Run();
