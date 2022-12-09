using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volunteer_Corner.Data.Migrations
{
    public partial class RequestResponseCommunication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HelpProposalResponses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HelpProposalToId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HelpSeekerFromId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IncludedHelpRequestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedByVolunteer = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpProposalResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HelpProposalResponses_HelpProposals_HelpProposalToId",
                        column: x => x.HelpProposalToId,
                        principalTable: "HelpProposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HelpProposalResponses_HelpRequests_IncludedHelpRequestId",
                        column: x => x.IncludedHelpRequestId,
                        principalTable: "HelpRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HelpProposalResponses_HelpSeekers_HelpSeekerFromId",
                        column: x => x.HelpSeekerFromId,
                        principalTable: "HelpSeekers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HelpRequestResponses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HelpRequestToId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VolunteerFromId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IncludedHelpProposalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedByHelpSeeker = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpRequestResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HelpRequestResponses_HelpProposals_IncludedHelpProposalId",
                        column: x => x.IncludedHelpProposalId,
                        principalTable: "HelpProposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HelpRequestResponses_HelpRequests_HelpRequestToId",
                        column: x => x.HelpRequestToId,
                        principalTable: "HelpRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HelpRequestResponses_Volunteers_VolunteerFromId",
                        column: x => x.VolunteerFromId,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HelpProposalResponses_HelpProposalToId",
                table: "HelpProposalResponses",
                column: "HelpProposalToId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpProposalResponses_HelpSeekerFromId",
                table: "HelpProposalResponses",
                column: "HelpSeekerFromId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpProposalResponses_IncludedHelpRequestId",
                table: "HelpProposalResponses",
                column: "IncludedHelpRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpRequestResponses_HelpRequestToId",
                table: "HelpRequestResponses",
                column: "HelpRequestToId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpRequestResponses_IncludedHelpProposalId",
                table: "HelpRequestResponses",
                column: "IncludedHelpProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpRequestResponses_VolunteerFromId",
                table: "HelpRequestResponses",
                column: "VolunteerFromId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelpProposalResponses");

            migrationBuilder.DropTable(
                name: "HelpRequestResponses");
        }
    }
}
