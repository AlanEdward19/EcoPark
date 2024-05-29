using EcoPark.Infrastructure;
using EcoPark.Presentation.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .ConfigureController()
    .ConfigureIoC(builder.Configuration)
    .ConfigureCors()
    .ConfigureAuthentication(builder.Configuration)
    .ConfigureSwagger()
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

app.UseWebSockets();


app.Run();
