using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volunteer_Corner.Data.Migrations
{
    public partial class Locations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "HelpRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "HelpProposals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "HelpRequests");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "HelpProposals");
        }
    }
}
