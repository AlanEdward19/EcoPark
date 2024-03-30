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
    [Migration("20240327182035_DatabaseStructure")]
    partial class DatabaseStructure
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EcoPark.Domain.Entities.Cars.Car", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("EcoPark.Domain.Entities.Locations.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("EcoPark.Domain.Entities.ParkingSpaces.ParkingSpace", b =>
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

            modelBuilder.Entity("EcoPark.Domain.Entities.Reservations.Reservation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ParkingSpaceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ReservationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ParkingSpaceId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("EcoPark.Domain.Entities.Users.User", b =>
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

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EcoPark.Domain.Entities.Cars.Car", b =>
                {
                    b.HasOne("EcoPark.Domain.Entities.Users.User", "User")
                        .WithMany("Cars")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EcoPark.Domain.Entities.ParkingSpaces.ParkingSpace", b =>
                {
                    b.HasOne("EcoPark.Domain.Entities.Locations.Location", "Location")
                        .WithMany("ParkingSpaces")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("EcoPark.Domain.Entities.Reservations.Reservation", b =>
                {
                    b.HasOne("EcoPark.Domain.Entities.ParkingSpaces.ParkingSpace", "ParkingSpace")
                        .WithMany("Reservations")
                        .HasForeignKey("ParkingSpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EcoPark.Domain.Entities.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParkingSpace");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EcoPark.Domain.Entities.Locations.Location", b =>
                {
                    b.Navigation("ParkingSpaces");
                });

            modelBuilder.Entity("EcoPark.Domain.Entities.ParkingSpaces.ParkingSpace", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("EcoPark.Domain.Entities.Users.User", b =>
                {
                    b.Navigation("Cars");
                });
#pragma warning restore 612, 618
        }
    }
}
