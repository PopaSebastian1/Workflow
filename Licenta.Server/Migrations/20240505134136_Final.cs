using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Licenta.Server.Migrations
{
    /// <inheritdoc />
    public partial class Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectAdministrators",
                columns: table => new
                {
                    AdministratorProjectsProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdministratorsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAdministrators", x => new { x.AdministratorProjectsProjectId, x.AdministratorsId });
                    table.ForeignKey(
                        name: "FK_ProjectAdministrators_AspNetUsers_AdministratorsId",
                        column: x => x.AdministratorsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectAdministrators_Projects_AdministratorProjectsProject~",
                        column: x => x.AdministratorProjectsProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAdministrators_AdministratorsId",
                table: "ProjectAdministrators",
                column: "AdministratorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectAdministrators");
        }
    }
}
