using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Licenta.Server.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectIssueStatuses",
                columns: table => new
                {
                    IssueStatusesId = table.Column<int>(type: "integer", nullable: false),
                    ProjectsProjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectIssueStatuses", x => new { x.IssueStatusesId, x.ProjectsProjectId });
                    table.ForeignKey(
                        name: "FK_ProjectIssueStatuses_IssuesStatus_IssueStatusesId",
                        column: x => x.IssueStatusesId,
                        principalTable: "IssuesStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectIssueStatuses_Projects_ProjectsProjectId",
                        column: x => x.ProjectsProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssueStatuses_ProjectsProjectId",
                table: "ProjectIssueStatuses",
                column: "ProjectsProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectIssueStatuses");
        }
    }
}
