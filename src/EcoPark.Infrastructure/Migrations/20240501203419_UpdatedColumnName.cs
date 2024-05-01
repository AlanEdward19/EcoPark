using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupAccesses_Employees_EmployeeModelId",
                table: "GroupAccesses");

            migrationBuilder.DropIndex(
                name: "IX_GroupAccesses_EmployeeModelId",
                table: "GroupAccesses");

            migrationBuilder.DropColumn(
                name: "EmployeeModelId",
                table: "GroupAccesses");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "GroupAccesses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccesses_EmployeeId",
                table: "GroupAccesses",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupAccesses_Employees_EmployeeId",
                table: "GroupAccesses",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupAccesses_Employees_EmployeeId",
                table: "GroupAccesses");

            migrationBuilder.DropIndex(
                name: "IX_GroupAccesses_EmployeeId",
                table: "GroupAccesses");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "GroupAccesses");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeModelId",
                table: "GroupAccesses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccesses_EmployeeModelId",
                table: "GroupAccesses",
                column: "EmployeeModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupAccesses_Employees_EmployeeModelId",
                table: "GroupAccesses",
                column: "EmployeeModelId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
