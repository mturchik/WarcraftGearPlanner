using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarcraftGearPlanner.Server.Data.Entities;

[Index("ItemClassId", "SubclassId", IsUnique = true)]
public class ItemSubclassEntity : BaseEntity
{
	public int SubclassId { get; set; }
	public string? Name { get; set; }
	public string? VerboseName { get; set; }
	public bool HideTooltip { get; set; }

	[ForeignKey(nameof(ItemClass))]
	public Guid ItemClassId { get; set; }
	public ItemClassEntity? ItemClass { get; set; }

	[InverseProperty(nameof(ItemEntity.ItemSubclass))]
	public List<ItemEntity>? Items { get; set; }

	[InverseProperty(nameof(ItemSubclassInventoryTypeEntity.ItemSubclass))]
	public List<ItemSubclassInventoryTypeEntity>? ItemSubclassInventoryTypes { get; set; }
}
