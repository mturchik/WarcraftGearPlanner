using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarcraftGearPlanner.Server.Migrations;

/// <inheritdoc />
public partial class AddItemSubclass : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "ItemSubclasses",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				SubclassId = table.Column<int>(type: "int", nullable: false),
				Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
				ItemClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_ItemSubclasses", x => x.Id);
				table.ForeignKey(
					name: "FK_ItemSubclasses_ItemClasses_ItemClassId",
					column: x => x.ItemClassId,
					principalTable: "ItemClasses",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "IX_ItemSubclasses_ItemClassId",
			table: "ItemSubclasses",
			column: "ItemClassId");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "ItemSubclasses");
	}
}
