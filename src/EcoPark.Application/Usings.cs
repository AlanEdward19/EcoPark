global using Microsoft.Extensions.DependencyInjection;
global using System.Text.Json.Serialization;

#region Domain

global using EcoPark.Domain.Interfaces;
global using EcoPark.Domain.Commons.Enums;
global using EcoPark.Domain.DataModels;
global using EcoPark.Domain.Commons.Base;
global using EcoPark.Domain.Interfaces.Database;
global using EcoPark.Domain.Interfaces.Services;

#endregion

#region Application

global using EcoPark.Application.Commons.Models;
global using EcoPark.Application.Locations.Models;
global using EcoPark.Application.ParkingSpaces.Models;
global using EcoPark.Application.Reservations.Models;
global using EcoPark.Application.Employees.Models;
global using EcoPark.Application.Cars.Models;
global using EcoPark.Application.Clients.Models;
global using EcoPark.Application.Locations.Delete;
global using EcoPark.Application.Locations.Get;
global using EcoPark.Application.Locations.Insert;
global using EcoPark.Application.Locations.List;
global using EcoPark.Application.Locations.Update;
global using EcoPark.Application.ParkingSpaces.Delete;
global using EcoPark.Application.ParkingSpaces.Get;
global using EcoPark.Application.ParkingSpaces.Insert;
global using EcoPark.Application.ParkingSpaces.List;
global using EcoPark.Application.ParkingSpaces.Update;
global using EcoPark.Application.Reservations.Delete;
global using EcoPark.Application.Reservations.Get;
global using EcoPark.Application.Reservations.Insert;
global using EcoPark.Application.Reservations.List;
global using EcoPark.Application.Reservations.Update;
global using EcoPark.Application.Employees.Delete;
global using EcoPark.Application.Employees.Get;
global using EcoPark.Application.Employees.Insert;
global using EcoPark.Application.Employees.List;
global using EcoPark.Application.Employees.Update;
global using EcoPark.Application.Authentication;

global using EcoPark.Application.Commons.Base.Commands;

#endregion