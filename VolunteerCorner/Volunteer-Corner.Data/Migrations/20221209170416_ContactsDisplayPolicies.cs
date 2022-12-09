using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volunteer_Corner.Data.Migrations
{
    public partial class ContactsDisplayPolicies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactsDisplayPolicy",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 1000);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactsDisplayPolicy",
                table: "AspNetUsers");
        }
    }
}
