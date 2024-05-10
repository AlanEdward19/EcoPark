using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ClientRewardDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientClaimedRewardModel_Clients_ClientId",
                table: "ClientClaimedRewardModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientClaimedRewardModel_Rewards_RewardId",
                table: "ClientClaimedRewardModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientClaimedRewardModel",
                table: "ClientClaimedRewardModel");

            migrationBuilder.RenameTable(
                name: "ClientClaimedRewardModel",
                newName: "ClientClaimedRewards");

            migrationBuilder.RenameIndex(
                name: "IX_ClientClaimedRewardModel_RewardId",
                table: "ClientClaimedRewards",
                newName: "IX_ClientClaimedRewards_RewardId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientClaimedRewardModel_ClientId",
                table: "ClientClaimedRewards",
                newName: "IX_ClientClaimedRewards_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientClaimedRewards",
                table: "ClientClaimedRewards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientClaimedRewards_Clients_ClientId",
                table: "ClientClaimedRewards",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientClaimedRewards_Rewards_RewardId",
                table: "ClientClaimedRewards",
                column: "RewardId",
                principalTable: "Rewards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientClaimedRewards_Clients_ClientId",
                table: "ClientClaimedRewards");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientClaimedRewards_Rewards_RewardId",
                table: "ClientClaimedRewards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientClaimedRewards",
                table: "ClientClaimedRewards");

            migrationBuilder.RenameTable(
                name: "ClientClaimedRewards",
                newName: "ClientClaimedRewardModel");

            migrationBuilder.RenameIndex(
                name: "IX_ClientClaimedRewards_RewardId",
                table: "ClientClaimedRewardModel",
                newName: "IX_ClientClaimedRewardModel_RewardId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientClaimedRewards_ClientId",
                table: "ClientClaimedRewardModel",
                newName: "IX_ClientClaimedRewardModel_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientClaimedRewardModel",
                table: "ClientClaimedRewardModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientClaimedRewardModel_Clients_ClientId",
                table: "ClientClaimedRewardModel",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientClaimedRewardModel_Rewards_RewardId",
                table: "ClientClaimedRewardModel",
                column: "RewardId",
                principalTable: "Rewards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
