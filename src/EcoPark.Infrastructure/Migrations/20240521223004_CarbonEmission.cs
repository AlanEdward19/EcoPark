using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CarbonEmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarbonEmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Forecast = table.Column<double>(type: "float", nullable: false),
                    Emission = table.Column<double>(type: "float", nullable: false),
                    Inhibition = table.Column<double>(type: "float", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarbonEmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarbonEmissions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CarbonEmissions_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarbonEmissions_ClientId",
                table: "CarbonEmissions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CarbonEmissions_ReservationId",
                table: "CarbonEmissions",
                column: "ReservationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarbonEmissions");
        }
    }
}
