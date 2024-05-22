global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.EntityFrameworkCore;

#region Domain

global using EcoPark.Domain.Interfaces;
global using EcoPark.Domain.Interfaces.Database;
global using EcoPark.Domain.DataModels;
global using EcoPark.Domain.DataModels.Client;
global using EcoPark.Domain.DataModels.Employee;
global using EcoPark.Domain.DataModels.Employee.Location;
global using EcoPark.Domain.DataModels.Employee.Location.ParkingSpace;
global using EcoPark.Domain.Aggregates.Client;
global using EcoPark.Domain.Commons.Enums;
global using EcoPark.Domain.Interfaces.Services;
global using EcoPark.Domain.Aggregates.Location;
global using EcoPark.Domain.ValueObjects;

#endregion

#region Application

global using EcoPark.Application.Authentication.Get;
global using EcoPark.Application.Punctuations;

#endregion

#region Infrastructure

global using EcoPark.Infrastructure.Data;

#endregion