using EcoPark.Application.Authentication.Get;
using EcoPark.Application.Authentication.Models;
using EcoPark.Application.Authentication.Services;
using EcoPark.Application.Cars.Delete;
using EcoPark.Application.Cars.Get;
using EcoPark.Application.Cars.Insert;
using EcoPark.Application.Cars.List;
using EcoPark.Application.Cars.Update;
using EcoPark.Application.Clients.Delete;
using EcoPark.Application.Clients.Get;
using EcoPark.Application.Clients.Insert;
using EcoPark.Application.Clients.List;
using EcoPark.Application.Clients.Update;
using EcoPark.Application.Employees.Delete.GroupAccess;
using EcoPark.Application.Employees.Insert.GroupAccess;
using EcoPark.Application.ParkingSpaces.Update.Status;
using EcoPark.Application.Punctuation.Get;
using EcoPark.Application.Punctuation.List;
using EcoPark.Application.Punctuation.Models;
using EcoPark.Application.Reservations.Update.Status;
using EcoPark.Domain.Interfaces.Services;

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
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }

    public static IServiceCollection ConfigureQueries(this IServiceCollection services)
    {
        services.AddScoped<IHandler<GetLocationQuery, LocationSimplifiedViewModel?>, GetLocationQueryHandler>();
        services.AddScoped<IHandler<ListLocationQuery, IEnumerable<LocationSimplifiedViewModel>>, ListLocationsQueryHandler>();

        services.AddScoped<IHandler<GetParkingSpaceQuery, ParkingSpaceSimplifiedViewModel?>, GetParkingSpaceQueryHandler>();
        services.AddScoped<IHandler<ListParkingSpacesQuery, IEnumerable<ParkingSpaceSimplifiedViewModel>?>, ListParkingSpacesQueryHandler>();

        services.AddScoped<IHandler<GetReservationQuery, ReservationSimplifiedViewModel?>, GetReservationQueryHandler>();
        services.AddScoped<IHandler<ListReservationQuery, IEnumerable<ReservationSimplifiedViewModel>>, ListReservationsQueryHandler>();

        services.AddScoped<IHandler<GetEmployeeQuery, EmployeeViewModel?>, GetEmployeeQueryHandler>();
        services.AddScoped<IHandler<ListEmployeesQuery, IEnumerable<EmployeeViewModel>>, ListEmployeesQueryHandler>();

        services.AddScoped<IHandler<GetClientQuery, ClientSimplifiedViewModel?>, GetClientQueryHandler>();
        services.AddScoped<IHandler<ListClientsQuery, IEnumerable<ClientSimplifiedViewModel>>, ListClientsQueryHandler>();

        services.AddScoped<IHandler<LoginQuery, LoginViewModel?>, LoginQueryHandler>();

        services.AddScoped<IHandler<GetCarQuery, CarViewModel?>, GetCarQueryHandler>();
        services.AddScoped<IHandler<ListCarQuery, IEnumerable<CarViewModel>>, ListCarQueryHandler>();

        services.AddScoped<IHandler<GetPunctuationQuery, PunctuationViewModel?>, GetPunctuationQueryHandler>();
        services.AddScoped<IHandler<ListPunctuationsQuery, IEnumerable<PunctuationViewModel>?>, ListPunctuationsQueryHandler>();

        return services;
    }

    public static IServiceCollection ConfigureCommands(this IServiceCollection services)
    {
        #region Location

        services
            .AddScoped<IHandler<InsertLocationCommand, DatabaseOperationResponseViewModel>,
                InsertLocationCommandHandler>();

        services
            .AddScoped<IHandler<UpdateLocationCommand, DatabaseOperationResponseViewModel>,
                UpdateLocationCommandHandler>();

        services
            .AddScoped<IHandler<DeleteLocationCommand, DatabaseOperationResponseViewModel>,
                DeleteLocationCommandHandler>();

        #endregion

        #region ParkingSpace

        services
            .AddScoped<IHandler<InsertParkingSpaceCommand, DatabaseOperationResponseViewModel>,
                InsertParkingSpaceCommandHandler>();

        services
            .AddScoped<IHandler<UpdateParkingSpaceCommand, DatabaseOperationResponseViewModel>,
                UpdateParkingSpaceCommandHandler>();

        services
            .AddScoped<IHandler<UpdateParkingSpaceStatusCommand, DatabaseOperationResponseViewModel>,
                UpdateParkingSpaceStatusCommandHandler>();

        services
            .AddScoped<IHandler<DeleteParkingSpaceCommand, DatabaseOperationResponseViewModel>,
                DeleteParkingSpaceCommandHandler>();

        #endregion

        #region Reservation

        services
            .AddScoped<IHandler<InsertReservationCommand, DatabaseOperationResponseViewModel>,
                InsertReservationCommandHandler>();
        services
            .AddScoped<IHandler<UpdateReservationCommand, DatabaseOperationResponseViewModel>,
                UpdateReservationCommandHandler>();
        services
            .AddScoped<IHandler<UpdateReservationStatusCommand, DatabaseOperationResponseViewModel>, 
                UpdateReservationStatusCommandHandler>();
        services
            .AddScoped<IHandler<DeleteReservationCommand, DatabaseOperationResponseViewModel>,
                DeleteReservationCommandHandler>();

        #endregion

        #region Employee

        services
            .AddScoped<IHandler<InsertEmployeeCommand, DatabaseOperationResponseViewModel>,
                InsertEmployeeCommandHandler>();

        services
            .AddScoped<IHandler<InsertEmployeeGroupAccessCommand, DatabaseOperationResponseViewModel>,
                           InsertEmployeeGroupAccessCommandHandler>();

        services
            .AddScoped<IHandler<UpdateEmployeeCommand, DatabaseOperationResponseViewModel>,
                UpdateEmployeeCommandHandler>();

        services
            .AddScoped<IHandler<DeleteEmployeeCommand, DatabaseOperationResponseViewModel>,
                DeleteEmployeeCommandHandler>();

        services
            .AddScoped<IHandler<DeleteEmployeeGroupAccessCommand, DatabaseOperationResponseViewModel>,
                           DeleteEmployeeGroupAccessCommandHandler>();

        #endregion

        #region Client

        services
            .AddScoped<IHandler<InsertClientCommand, DatabaseOperationResponseViewModel>, InsertClientCommandHandler>();

        services
            .AddScoped<IHandler<UpdateClientCommand, DatabaseOperationResponseViewModel>, UpdateClientCommandHandler>();

        services
            .AddScoped<IHandler<DeleteClientCommand, DatabaseOperationResponseViewModel>, DeleteClientCommandHandler>();

        #endregion

        #region Car

        services.AddScoped<IHandler<InsertCarCommand, DatabaseOperationResponseViewModel>, InsertCarCommandHandler>();

        services.AddScoped<IHandler<UpdateCarCommand, DatabaseOperationResponseViewModel>, UpdateCarCommandHandler>();

        services.AddScoped<IHandler<DeleteCarCommand, DatabaseOperationResponseViewModel>, DeleteCarCommandHandler>();

        #endregion

        return services;
    }
}