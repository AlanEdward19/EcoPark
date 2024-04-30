using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseRefactor2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Administrators_AdministratorId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Administrators_OwnerId",
                table: "Locations");

            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_AdministratorId",
                table: "Employees",
                column: "AdministratorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Employees_OwnerId",
                table: "Locations",
                column: "OwnerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_AdministratorId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Employees_OwnerId",
                table: "Locations");

            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CredentialsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administrators_Credentials_CredentialsId",
                        column: x => x.CredentialsId,
                        principalTable: "Credentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administrators_CredentialsId",
                table: "Administrators",
                column: "CredentialsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Administrators_AdministratorId",
                table: "Employees",
                column: "AdministratorId",
                principalTable: "Administrators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Administrators_OwnerId",
                table: "Locations",
                column: "OwnerId",
                principalTable: "Administrators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
