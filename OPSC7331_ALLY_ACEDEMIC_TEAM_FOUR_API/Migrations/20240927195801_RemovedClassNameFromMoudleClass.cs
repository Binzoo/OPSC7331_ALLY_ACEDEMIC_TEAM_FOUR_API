using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Migrations
{
    /// <inheritdoc />
    public partial class RemovedClassNameFromMoudleClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "ModuleClasses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "ModuleClasses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
