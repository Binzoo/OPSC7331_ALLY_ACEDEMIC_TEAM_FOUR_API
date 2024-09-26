using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Migrations
{
    /// <inheritdoc />
    public partial class changeNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlaceName",
                table: "SchoolNavigations",
                newName: "StartPlace");

            migrationBuilder.AddColumn<string>(
                name: "EndPlace",
                table: "SchoolNavigations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndPlace",
                table: "SchoolNavigations");

            migrationBuilder.RenameColumn(
                name: "StartPlace",
                table: "SchoolNavigations",
                newName: "PlaceName");
        }
    }
}
