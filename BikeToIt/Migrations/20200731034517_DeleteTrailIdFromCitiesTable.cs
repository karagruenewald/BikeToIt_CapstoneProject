using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeToIt.Migrations
{
    public partial class DeleteTrailIdFromCitiesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Trails_TrailId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_TrailId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "TrailId",
                table: "Cities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrailId",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_TrailId",
                table: "Cities",
                column: "TrailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Trails_TrailId",
                table: "Cities",
                column: "TrailId",
                principalTable: "Trails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
