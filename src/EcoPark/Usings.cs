global using Microsoft.AspNetCore.Http.Extensions;
global using Microsoft.AspNetCore.Mvc;

#region Domain

global using EcoPark.Domain.Entities.Locations;
global using EcoPark.Domain.Entities.ParkingSpaces;
global using EcoPark.Domain.Entities.Reservations;
global using EcoPark.Domain.Entities.Users;
global using EcoPark.Domain.Interfaces;

#endregion

#region Application

global using EcoPark.Application.Reservations.Insert;
global using EcoPark.Application.Reservations.Update;
global using EcoPark.Application.ParkingSpaces.Insert;
global using EcoPark.Application.ParkingSpaces.Update;
global using EcoPark.Application.Locations.Insert;
global using EcoPark.Application.Locations.Update;
global using EcoPark.Application.Users.Insert;
global using EcoPark.Application.Users.Update;
global using EcoPark.Application.Commons.Models;

#endregion