﻿using Microsoft.EntityFrameworkCore;

namespace EcoPark.Infrastructure.Data;

public partial class DatabaseDbContext
{
    public DbSet<LocationModel> Locations { get; set; }
    public DbSet<ReservationModel> Reservations { get; set; }
    public DbSet<ParkingSpaceModel> ParkingSpaces { get; set; }
    public DbSet<ClientModel> Clients { get; set; }
    public DbSet<EmployeeModel> Employees { get; set; }
    public DbSet<CarModel> Cars { get; set; }
}