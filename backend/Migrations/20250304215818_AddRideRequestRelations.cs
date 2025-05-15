using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddRideRequestRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RideRequests_RouteId",
                table: "RideRequests",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RideRequests_UserId",
                table: "RideRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideRequests_Routes_RouteId",
                table: "RideRequests",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RideRequests_Users_UserId",
                table: "RideRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideRequests_Routes_RouteId",
                table: "RideRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RideRequests_Users_UserId",
                table: "RideRequests");

            migrationBuilder.DropIndex(
                name: "IX_RideRequests_RouteId",
                table: "RideRequests");

            migrationBuilder.DropIndex(
                name: "IX_RideRequests_UserId",
                table: "RideRequests");
        }
    }
}
