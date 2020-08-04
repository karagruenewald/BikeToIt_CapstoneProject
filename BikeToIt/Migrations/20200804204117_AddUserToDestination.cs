using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeToIt.Migrations
{
    public partial class AddUserToDestination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Destinations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Destinations");
        }
    }
}
