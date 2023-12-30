using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarcraftGearPlanner.Server.Migrations;

/// <inheritdoc />
public partial class AddItemReferenceIndexes : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "IX_ItemSubclasses_ItemClassId",
			table: "ItemSubclasses");

		migrationBuilder.AlterColumn<string>(
			name: "Type",
			table: "ItemQualities",
			type: "nvarchar(450)",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "nvarchar(max)");

		migrationBuilder.AlterColumn<string>(
			name: "Name",
			table: "ItemQualities",
			type: "nvarchar(450)",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "nvarchar(max)");

		migrationBuilder.AlterColumn<string>(
			name: "Type",
			table: "InventoryTypes",
			type: "nvarchar(450)",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "nvarchar(max)");

		migrationBuilder.AlterColumn<string>(
			name: "Name",
			table: "InventoryTypes",
			type: "nvarchar(450)",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "nvarchar(max)");

		migrationBuilder.CreateIndex(
			name: "IX_ItemSubclasses_ItemClassId_SubclassId",
			table: "ItemSubclasses",
			columns: new[] { "ItemClassId", "SubclassId" },
			unique: true);

		migrationBuilder.CreateIndex(
			name: "IX_ItemQualities_Type_Name",
			table: "ItemQualities",
			columns: new[] { "Type", "Name" },
			unique: true);

		migrationBuilder.CreateIndex(
			name: "IX_ItemClasses_ClassId",
			table: "ItemClasses",
			column: "ClassId",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "IX_InventoryTypes_Type_Name",
			table: "InventoryTypes",
			columns: new[] { "Type", "Name" },
			unique: true);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "IX_ItemSubclasses_ItemClassId_SubclassId",
			table: "ItemSubclasses");

		migrationBuilder.DropIndex(
			name: "IX_ItemQualities_Type_Name",
			table: "ItemQualities");

		migrationBuilder.DropIndex(
			name: "IX_ItemClasses_ClassId",
			table: "ItemClasses");

		migrationBuilder.DropIndex(
			name: "IX_InventoryTypes_Type_Name",
			table: "InventoryTypes");

		migrationBuilder.AlterColumn<string>(
			name: "Type",
			table: "ItemQualities",
			type: "nvarchar(max)",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "nvarchar(450)");

		migrationBuilder.AlterColumn<string>(
			name: "Name",
			table: "ItemQualities",
			type: "nvarchar(max)",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "nvarchar(450)");

		migrationBuilder.AlterColumn<string>(
			name: "Type",
			table: "InventoryTypes",
			type: "nvarchar(max)",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "nvarchar(450)");

		migrationBuilder.AlterColumn<string>(
			name: "Name",
			table: "InventoryTypes",
			type: "nvarchar(max)",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "nvarchar(450)");

		migrationBuilder.CreateIndex(
			name: "IX_ItemSubclasses_ItemClassId",
			table: "ItemSubclasses",
			column: "ItemClassId");
	}
}
