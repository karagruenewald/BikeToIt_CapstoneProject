using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeToIt.Migrations
{
    public partial class AddedImageProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Trails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Destinations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Trails");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Destinations");
        }
    }
}
