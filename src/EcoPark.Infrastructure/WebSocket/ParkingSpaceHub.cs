﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EcoPark.Infrastructure.WebSocket;

public class ParkingSpaceHub(DatabaseDbContext databaseDbContext, ILogger<ParkingSpaceHub> logger) : Hub
{
    public async Task GetParkingSpaces(Guid locationId)
    {
        logger.LogInformation("WebSocket chamado");

        var parkingSpaces =
            await databaseDbContext.ParkingSpaces
                .Where(x => x.LocationId.Equals(locationId))
                .ToListAsync();

        await Clients.All.SendAsync("ReceiveParkingSpaces", parkingSpaces);
    }

    public override async Task OnConnectedAsync()
    {
        logger.LogInformation("Cliente conectado ao hub SignalR");

        await Clients.All.SendAsync("ReceiveMessages",$"{Context.ConnectionId} has joined");


    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation("Cliente desconectado do hub SignalR");
        return base.OnDisconnectedAsync(exception);
    }
}