using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoreDetailsToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FuelConsumptionPerLiter",
                table: "Cars",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "FuelType",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelConsumptionPerLiter",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "Cars");
        }
    }
}
