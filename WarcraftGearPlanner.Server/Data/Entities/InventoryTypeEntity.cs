using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarcraftGearPlanner.Server.Data.Entities;

[Index("Type", "Name", IsUnique = true)]
public class InventoryTypeEntity : BaseEntity
{
	public string Type { get; set; } = "";
	public string Name { get; set; } = "";
	public int? DisplayOrder { get; set; }

	[InverseProperty(nameof(ItemEntity.InventoryType))]
	public List<ItemEntity>? Items { get; set; }

	[InverseProperty(nameof(ItemSubclassInventoryTypeEntity.InventoryType))]
	public List<ItemSubclassInventoryTypeEntity>? ItemSubclassInventoryTypes { get; set; }
}
