using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarcraftGearPlanner.Server.Migrations;

/// <inheritdoc />
public partial class AddItemEntities : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "InventoryTypes",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
				Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
				CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_InventoryTypes", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "ItemQualities",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
				Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
				CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_ItemQualities", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "Items",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				ItemId = table.Column<long>(type: "bigint", nullable: false),
				Level = table.Column<long>(type: "bigint", nullable: false),
				Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
				RequiredLevel = table.Column<long>(type: "bigint", nullable: false),
				MaxCount = table.Column<long>(type: "bigint", nullable: false),
				PurchaseQuantity = table.Column<long>(type: "bigint", nullable: false),
				PurchasePrice = table.Column<long>(type: "bigint", nullable: false),
				SellPrice = table.Column<long>(type: "bigint", nullable: false),
				IsEquippable = table.Column<bool>(type: "bit", nullable: false),
				IsStackable = table.Column<bool>(type: "bit", nullable: false),
				ItemClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
				ItemSubclassId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
				ItemQualityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
				InventoryTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
				CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Items", x => x.Id);
				table.ForeignKey(
					name: "FK_Items_InventoryTypes_InventoryTypeId",
					column: x => x.InventoryTypeId,
					principalTable: "InventoryTypes",
					principalColumn: "Id");
				table.ForeignKey(
					name: "FK_Items_ItemClasses_ItemClassId",
					column: x => x.ItemClassId,
					principalTable: "ItemClasses",
					principalColumn: "Id");
				table.ForeignKey(
					name: "FK_Items_ItemQualities_ItemQualityId",
					column: x => x.ItemQualityId,
					principalTable: "ItemQualities",
					principalColumn: "Id");
				table.ForeignKey(
					name: "FK_Items_ItemSubclasses_ItemSubclassId",
					column: x => x.ItemSubclassId,
					principalTable: "ItemSubclasses",
					principalColumn: "Id");
			});

		migrationBuilder.CreateIndex(
			name: "IX_Items_InventoryTypeId",
			table: "Items",
			column: "InventoryTypeId");

		migrationBuilder.CreateIndex(
			name: "IX_Items_ItemClassId",
			table: "Items",
			column: "ItemClassId");

		migrationBuilder.CreateIndex(
			name: "IX_Items_ItemQualityId",
			table: "Items",
			column: "ItemQualityId");

		migrationBuilder.CreateIndex(
			name: "IX_Items_ItemSubclassId",
			table: "Items",
			column: "ItemSubclassId");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "Items");

		migrationBuilder.DropTable(
			name: "InventoryTypes");

		migrationBuilder.DropTable(
			name: "ItemQualities");
	}
}
