using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarcraftGearPlanner.Server.Migrations;

/// <inheritdoc />
public partial class AddItemSubclassInventoryTypeTable : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "ItemSubclassInventoryTypeEntity",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				ItemSubclassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				InventoryTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_ItemSubclassInventoryTypeEntity", x => x.Id);
				table.ForeignKey(
					name: "FK_ItemSubclassInventoryTypeEntity_InventoryTypes_InventoryTypeId",
					column: x => x.InventoryTypeId,
					principalTable: "InventoryTypes",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "FK_ItemSubclassInventoryTypeEntity_ItemSubclasses_ItemSubclassId",
					column: x => x.ItemSubclassId,
					principalTable: "ItemSubclasses",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "IX_ItemSubclassInventoryTypeEntity_InventoryTypeId",
			table: "ItemSubclassInventoryTypeEntity",
			column: "InventoryTypeId");

		migrationBuilder.CreateIndex(
			name: "IX_ItemSubclassInventoryTypeEntity_ItemSubclassId",
			table: "ItemSubclassInventoryTypeEntity",
			column: "ItemSubclassId");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "ItemSubclassInventoryTypeEntity");
	}
}
