using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volunteer_Corner.Data.Migrations
{
    public partial class ApprovingHelpSeekersAndVolunteers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Volunteers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "HelpSeekers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "HelpSeekers");
        }
    }
}
