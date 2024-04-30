using EcoPark.Infrastructure;
using EcoPark.Presentation.Configurations;
using Microsoft.OpenApi.Models;
using System.ComponentModel;

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
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "EcoPark.API",
            Version = "v1",
            Description = """
                          EcoPark é uma solução projetada para facilitar a gestão de estacionamentos, através de um sistema de reservas e controle de vagas.
                          Onde o usuário pode realizar reservas de vagas de estacionamento, receber pontos que podem ser trocados no estabelecimento e o administrador pode gerenciar as vagas disponíveis e as reservas realizadas.
                          Tudo isso de forma simples e intuitiva e também ajudando o meio ambiente diminuindo a pegada de carbono.
                          """

        });

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

        c.CustomSchemaIds(x => x.GetCustomAttributes(false).OfType<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ?? x.Name);

#if DEBUG
        Directory.GetFiles("./Configurations/Comments/", "*.xml", SearchOption.TopDirectoryOnly).ToList()
            .ForEach(xml => c.IncludeXmlComments(xml));
#endif

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
    .UpdateMigrations()
    .ConfigureMiddleware()
    .UseHttpsRedirection()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .ConfigureEndpoints(builder.Configuration.GetSection("EndPointsConfig"));

app.Run();
