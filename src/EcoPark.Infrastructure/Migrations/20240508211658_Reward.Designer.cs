﻿// <auto-generated />
using System;
using EcoPark.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EcoPark.Infrastructure.Migrations
{
    [DbContext(typeof(DatabaseDbContext))]
    [Migration("20240508211658_Reward")]
    partial class Reward
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EcoPark.Domain.DataModels.Client.CarModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Client.ClientClaimedRewardModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<Guid>("RewardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("RewardId");

                    b.ToTable("ClientClaimedRewardModel");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Client.ClientModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CredentialsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CredentialsId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Client.PunctuationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Punctuation")
                        .HasColumnType("float");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("LocationId");

                    b.ToTable("Punctuations");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.CredentialsModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ipv4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.EmployeeModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AdministratorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CredentialsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AdministratorId");

                    b.HasIndex("CredentialsId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.GroupAccessModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("LocationId")
                        .IsUnique();

                    b.ToTable("GroupAccesses");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.Location.LocationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("CancellationFeeRate")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("HourlyParkingRate")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("ReservationFeeRate")
                        .HasColumnType("float");

                    b.Property<int>("ReservationGraceInMinutes")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.Location.ParkingSpace.ParkingSpaceModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<bool>("IsOccupied")
                        .HasColumnType("bit");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ParkingSpaceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParkingSpaceType")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("ParkingSpaces");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.Location.ParkingSpace.ReservationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CarId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ParkingSpaceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Punctuation")
                        .HasColumnType("float");

                    b.Property<string>("ReservationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("ClientId");

                    b.HasIndex("ParkingSpaceId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.Location.RewardModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("AvailableQuantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("RequiredPoints")
                        .HasColumnType("float");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Rewards");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Client.CarModel", b =>
                {
                    b.HasOne("EcoPark.Domain.DataModels.Client.ClientModel", "Client")
                        .WithMany("Cars")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Client.ClientClaimedRewardModel", b =>
                {
                    b.HasOne("EcoPark.Domain.DataModels.Client.ClientModel", "Client")
                        .WithMany("ClaimedRewards")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EcoPark.Domain.DataModels.Employee.Location.RewardModel", "Reward")
                        .WithMany()
                        .HasForeignKey("RewardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Reward");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Client.ClientModel", b =>
                {
                    b.HasOne("EcoPark.Domain.DataModels.CredentialsModel", "Credentials")
                        .WithMany()
                        .HasForeignKey("CredentialsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Credentials");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Client.PunctuationModel", b =>
                {
                    b.HasOne("EcoPark.Domain.DataModels.Client.ClientModel", "Client")
                        .WithMany("Punctuations")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EcoPark.Domain.DataModels.Employee.Location.LocationModel", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.EmployeeModel", b =>
                {
                    b.HasOne("EcoPark.Domain.DataModels.Employee.EmployeeModel", "Administrator")
                        .WithMany("Employees")
                        .HasForeignKey("AdministratorId");

                    b.HasOne("EcoPark.Domain.DataModels.CredentialsModel", "Credentials")
                        .WithMany()
                        .HasForeignKey("CredentialsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Administrator");

                    b.Navigation("Credentials");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.GroupAccessModel", b =>
                {
                    b.HasOne("EcoPark.Domain.DataModels.Employee.EmployeeModel", "Employee")
                        .WithMany("GroupAccesses")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EcoPark.Domain.DataModels.Employee.Location.LocationModel", "Location")
                        .WithOne("GroupAccess")
                        .HasForeignKey("EcoPark.Domain.DataModels.Employee.GroupAccessModel", "LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.Location.LocationModel", b =>
                {
                    b.HasOne("EcoPark.Domain.DataModels.Employee.EmployeeModel", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.Location.ParkingSpace.ParkingSpaceModel", b =>
                {
                    b.HasOne("EcoPark.Domain.DataModels.Employee.Location.LocationModel", "Location")
                        .WithMany("ParkingSpaces")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.Location.ParkingSpace.ReservationModel", b =>
                {
                    b.HasOne("EcoPark.Domain.DataModels.Client.CarModel", "Car")
                        .WithMany("Reservations")
                        .HasForeignKey("CarId");

                    b.HasOne("EcoPark.Domain.DataModels.Client.ClientModel", "Client")
                        .WithMany("Reservations")
                        .HasForeignKey("ClientId");

                    b.HasOne("EcoPark.Domain.DataModels.Employee.Location.ParkingSpace.ParkingSpaceModel", "ParkingSpace")
                        .WithMany("Reservations")
                        .HasForeignKey("ParkingSpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Client");

                    b.Navigation("ParkingSpace");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.Location.RewardModel", b =>
                {
                    b.HasOne("EcoPark.Domain.DataModels.Employee.Location.LocationModel", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Client.CarModel", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Client.ClientModel", b =>
                {
                    b.Navigation("Cars");

                    b.Navigation("ClaimedRewards");

                    b.Navigation("Punctuations");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.EmployeeModel", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("GroupAccesses");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.Location.LocationModel", b =>
                {
                    b.Navigation("GroupAccess");

                    b.Navigation("ParkingSpaces");
                });

            modelBuilder.Entity("EcoPark.Domain.DataModels.Employee.Location.ParkingSpace.ParkingSpaceModel", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
