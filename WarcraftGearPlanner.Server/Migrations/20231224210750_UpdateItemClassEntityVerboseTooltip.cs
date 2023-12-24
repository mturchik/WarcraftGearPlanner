using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarcraftGearPlanner.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateItemClassEntityVerboseTooltip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HideTooltip",
                table: "ItemSubclasses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VerboseName",
                table: "ItemSubclasses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HideTooltip",
                table: "ItemSubclasses");

            migrationBuilder.DropColumn(
                name: "VerboseName",
                table: "ItemSubclasses");
        }
    }
}
