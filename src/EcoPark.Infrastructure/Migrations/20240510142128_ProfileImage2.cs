using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProfileImage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Credentials",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Credentials");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
