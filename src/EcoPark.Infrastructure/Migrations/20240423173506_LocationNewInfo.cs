using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LocationNewInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CancellationFeeRate",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HourlyParkingRate",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReservationFeeRate",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ReservationGraceInMinutes",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationFeeRate",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "HourlyParkingRate",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ReservationFeeRate",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ReservationGraceInMinutes",
                table: "Locations");
        }
    }
}
