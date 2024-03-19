namespace Application;

public static class ApplicationModule
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services
            .ConfigureServices()
            .ConfigureCommands()
            .ConfigureQueries();

        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection ConfigureQueries(this IServiceCollection services)
    {
        services.AddScoped<IHandler<Guid, Location>, GetLocationQueryHandler>();
        services.AddScoped<IHandler<IEnumerable<Guid>?, IEnumerable<Location>>, ListLocationsQueryHandler>();

        services.AddScoped<IHandler<Guid, ParkingSpace>, GetParkingSpaceQueryHandler>();
        services.AddScoped<IHandler<IEnumerable<Guid>?, IEnumerable<ParkingSpace>>, ListParkingSpacesQueryHandler>();

        services.AddScoped<IHandler<Guid, Reservation>, GetReservationQueryHandler>();
        services.AddScoped<IHandler<IEnumerable<Guid>?, IEnumerable<Reservation>>, ListReservationsQueryHandler>();

        return services;
    }

    public static IServiceCollection ConfigureCommands(this IServiceCollection services)
    {
        services.AddScoped<IHandler<InsertLocationCommand, Guid>, InsertLocationCommandHandler>();
        services.AddScoped<IHandler<UpdateLocationCommand, Guid>, UpdateLocationCommandHandler>();
        services.AddScoped<IHandler<Guid, bool>, DeleteLocationCommandHandler>();

        services.AddScoped<IHandler<InsertParkingSpaceCommand, Guid>, InsertParkingSpaceCommandHandler>();
        services.AddScoped<IHandler<UpdateParkingSpaceCommand, Guid>, UpdateParkingSpaceCommandHandler>();
        services.AddScoped<IHandler<Guid, bool>, DeleteParkingSpaceCommandHandler>();

        services.AddScoped<IHandler<InsertReservationCommand, Guid>, InsertReservationCommandHandler>();
        services.AddScoped<IHandler<UpdateReservationCommand, Guid>, UpdateReservationCommandHandler>();
        services.AddScoped<IHandler<Guid, bool>, DeleteReservationCommandHandler>();

        return services;
    }
}