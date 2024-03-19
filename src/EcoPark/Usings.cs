global using Microsoft.AspNetCore.Http.Extensions;
global using Microsoft.AspNetCore.Mvc;

#region Domain

global using Domain.Entities.Locations;
global using Domain.Entities.ParkingSpaces;
global using Domain.Entities.Reservations;
global using Domain.Interfaces;

#endregion

#region Application

global using Application.Reservations.Insert;
global using Application.Reservations.Update;
global using Application.ParkingSpaces.Insert;
global using Application.ParkingSpaces.Update;
global using Application.Locations.Insert;
global using Application.Locations.Update;

#endregion