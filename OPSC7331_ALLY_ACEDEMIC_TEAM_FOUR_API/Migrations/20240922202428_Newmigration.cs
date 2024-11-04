using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Migrations
{
    /// <inheritdoc />
    public partial class Newmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeWork_Modules_ModuleID",
                table: "HomeWork");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HomeWork",
                table: "HomeWork");

            migrationBuilder.RenameTable(
                name: "HomeWork",
                newName: "HomeWorks");

            migrationBuilder.RenameIndex(
                name: "IX_HomeWork_ModuleID",
                table: "HomeWorks",
                newName: "IX_HomeWorks_ModuleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HomeWorks",
                table: "HomeWorks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeWorks_Modules_ModuleID",
                table: "HomeWorks",
                column: "ModuleID",
                principalTable: "Modules",
                principalColumn: "ModuleID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeWorks_Modules_ModuleID",
                table: "HomeWorks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HomeWorks",
                table: "HomeWorks");

            migrationBuilder.RenameTable(
                name: "HomeWorks",
                newName: "HomeWork");

            migrationBuilder.RenameIndex(
                name: "IX_HomeWorks_ModuleID",
                table: "HomeWork",
                newName: "IX_HomeWork_ModuleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HomeWork",
                table: "HomeWork",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeWork_Modules_ModuleID",
                table: "HomeWork",
                column: "ModuleID",
                principalTable: "Modules",
                principalColumn: "ModuleID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
