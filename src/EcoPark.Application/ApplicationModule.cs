namespace EcoPark.Application;

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
        services.AddScoped<IHandler<GetLocationQuery, LocationSimplifiedViewModel?>, GetLocationQueryHandler>();
        services.AddScoped<IHandler<ListLocationQuery, IEnumerable<LocationSimplifiedViewModel>>, ListLocationsQueryHandler>();

        services.AddScoped<IHandler<GetParkingSpaceQuery, ParkingSpaceSimplifiedViewModel?>, GetParkingSpaceQueryHandler>();
        services.AddScoped<IHandler<ListParkingSpacesQuery, IEnumerable<ParkingSpaceSimplifiedViewModel>?>, ListParkingSpacesQueryHandler>();

        services.AddScoped<IHandler<GetReservationQuery, ReservationSimplifiedViewModel?>, GetReservationQueryHandler>();
        services.AddScoped<IHandler<IEnumerable<Guid>?, IEnumerable<ReservationModel>>, ListReservationsQueryHandler>();

        return services;
    }

    public static IServiceCollection ConfigureCommands(this IServiceCollection services)
    {
        services
            .AddScoped<IHandler<InsertLocationCommand, DatabaseOperationResponseViewModel>,
                InsertLocationCommandHandler>();
        services
            .AddScoped<IHandler<UpdateLocationCommand, DatabaseOperationResponseViewModel>,
                UpdateLocationCommandHandler>();
        services
            .AddScoped<IHandler<DeleteLocationCommand, DatabaseOperationResponseViewModel>,
                DeleteLocationCommandHandler>();

        services
            .AddScoped<IHandler<InsertParkingSpaceCommand, DatabaseOperationResponseViewModel>,
                InsertParkingSpaceCommandHandler>();
        services
            .AddScoped<IHandler<UpdateParkingSpaceCommand, DatabaseOperationResponseViewModel>,
                UpdateParkingSpaceCommandHandler>();
        services
            .AddScoped<IHandler<DeleteParkingSpaceCommand, DatabaseOperationResponseViewModel>,
                DeleteParkingSpaceCommandHandler>();

        services
            .AddScoped<IHandler<InsertReservationCommand, DatabaseOperationResponseViewModel>,
                InsertReservationCommandHandler>();
        services
            .AddScoped<IHandler<UpdateReservationCommand, DatabaseOperationResponseViewModel>,
                UpdateReservationCommandHandler>();
        services
            .AddScoped<IHandler<DeleteReservationCommand, DatabaseOperationResponseViewModel>,
                DeleteReservationCommandHandler>();

        return services;
    }
}