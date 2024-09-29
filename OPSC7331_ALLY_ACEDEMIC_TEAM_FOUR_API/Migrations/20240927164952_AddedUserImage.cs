using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImageUrl",
                table: "AspNetUsers");
        }
    }
}
