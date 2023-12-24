using System.ComponentModel.DataAnnotations.Schema;

namespace WarcraftGearPlanner.Server.Data.Entities;

public class ItemClassEntity : BaseEntity
{
	public int ClassId { get; set; }
	public string? Name { get; set; }

	[InverseProperty(nameof(ItemSubclassEntity.ItemClass))]
	public List<ItemSubclassEntity>? Subclasses { get; set; }
}
