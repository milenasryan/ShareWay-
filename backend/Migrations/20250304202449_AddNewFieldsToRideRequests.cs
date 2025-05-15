using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToRideRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "RideRequests",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "RideRequests",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "RideRequests");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RideRequests",
                newName: "PassengerId");
        }
    }
}
