using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PunctuationFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Punctuations_Clients_ClientModelId",
                table: "Punctuations");

            migrationBuilder.DropIndex(
                name: "IX_Punctuations_ClientModelId",
                table: "Punctuations");

            migrationBuilder.DropColumn(
                name: "ClientModelId",
                table: "Punctuations");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Punctuations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Punctuations_ClientId",
                table: "Punctuations",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Punctuations_Clients_ClientId",
                table: "Punctuations",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Punctuations_Clients_ClientId",
                table: "Punctuations");

            migrationBuilder.DropIndex(
                name: "IX_Punctuations_ClientId",
                table: "Punctuations");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Punctuations");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientModelId",
                table: "Punctuations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Punctuations_ClientModelId",
                table: "Punctuations",
                column: "ClientModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Punctuations_Clients_ClientModelId",
                table: "Punctuations",
                column: "ClientModelId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
