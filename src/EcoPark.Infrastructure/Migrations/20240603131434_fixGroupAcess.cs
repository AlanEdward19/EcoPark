using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixGroupAcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupAccesses",
                table: "GroupAccesses");

            migrationBuilder.DropIndex(
                name: "IX_GroupAccesses_EmployeeId",
                table: "GroupAccesses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupAccesses",
                table: "GroupAccesses",
                columns: new[] { "EmployeeId", "LocationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupAccesses",
                table: "GroupAccesses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupAccesses",
                table: "GroupAccesses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccesses_EmployeeId",
                table: "GroupAccesses",
                column: "EmployeeId");
        }
    }
}
