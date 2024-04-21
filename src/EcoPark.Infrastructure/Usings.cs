global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.EntityFrameworkCore;

#region Domain

global using EcoPark.Domain.DataModels;
global using EcoPark.Domain.Commons.Base;
global using EcoPark.Domain.Interfaces;
global using EcoPark.Domain.Interfaces.Database;
global using EcoPark.Domain.Interfaces.Services;
global using EcoPark.Domain.ValueObjects;

#endregion

#region Application

global using EcoPark.Application.Authentication.Get;

#endregion

#region Infrastructure

global using EcoPark.Infrastructure.Data;

#endregion