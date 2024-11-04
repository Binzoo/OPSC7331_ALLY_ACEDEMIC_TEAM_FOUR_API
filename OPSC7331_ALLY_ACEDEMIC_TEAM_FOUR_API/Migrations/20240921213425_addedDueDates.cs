using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Migrations
{
    /// <inheritdoc />
    public partial class addedDueDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "HomeWork",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "HomeWork");
        }
    }
}
