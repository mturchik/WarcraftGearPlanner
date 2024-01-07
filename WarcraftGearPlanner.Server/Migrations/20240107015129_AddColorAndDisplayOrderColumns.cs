using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarcraftGearPlanner.Server.Migrations;

/// <inheritdoc />
public partial class AddColorAndDisplayOrderColumns : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<int>(
			name: "DisplayOrder",
			table: "ItemSubclasses",
			type: "int",
			nullable: true);

		migrationBuilder.AddColumn<string>(
			name: "Color",
			table: "ItemQualities",
			type: "nvarchar(max)",
			nullable: true);

		migrationBuilder.AddColumn<int>(
			name: "DisplayOrder",
			table: "ItemQualities",
			type: "int",
			nullable: true);

		migrationBuilder.AddColumn<int>(
			name: "DisplayOrder",
			table: "ItemClasses",
			type: "int",
			nullable: true);

		migrationBuilder.AddColumn<int>(
			name: "DisplayOrder",
			table: "InventoryTypes",
			type: "int",
			nullable: true);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "DisplayOrder",
			table: "ItemSubclasses");

		migrationBuilder.DropColumn(
			name: "Color",
			table: "ItemQualities");

		migrationBuilder.DropColumn(
			name: "DisplayOrder",
			table: "ItemQualities");

		migrationBuilder.DropColumn(
			name: "DisplayOrder",
			table: "ItemClasses");

		migrationBuilder.DropColumn(
			name: "DisplayOrder",
			table: "InventoryTypes");
	}
}
