﻿using EcoPark.Application.Clients.Models;
using EcoPark.Domain.Commons.Base;
using EcoPark.Domain.Interfaces.Database;
using EcoPark.Infrastructure.Data;
using EcoPark.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace EcoPark.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureDatabase(configuration)
            .ConfigureWebSocket()
            .ConfigureRepositories();

        return services;
    }

    private static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext(configuration.GetConnectionString("MainDatabase"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAggregateRepository<LocationModel>, LocationRepository>();
        services.AddScoped<IAggregateRepository<ParkingSpaceModel>, ParkingSpaceRepository>();
        services.AddScoped<IRepository<EmployeeModel>, EmployeeRepository>();
        services.AddScoped<IRepository<ReservationModel>, ReservationRepository>();
        services.AddScoped<IRepository<UserModel>, LoginRepository>();
        services.AddScoped<IRepository<ClientSimplifiedViewModel>, ClientRepository>();

        return services;
    }

    private static IServiceCollection ConfigureWebSocket(this IServiceCollection services)
    {
        services
            .AddSignalR();

        return services;
    }
}