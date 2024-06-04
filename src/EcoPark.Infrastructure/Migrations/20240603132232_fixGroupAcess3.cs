using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixGroupAcess3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupAccesses_LocationId",
                table: "GroupAccesses");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccesses_LocationId",
                table: "GroupAccesses",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupAccesses_LocationId",
                table: "GroupAccesses");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccesses_LocationId",
                table: "GroupAccesses",
                column: "LocationId",
                unique: true);
        }
    }
}
