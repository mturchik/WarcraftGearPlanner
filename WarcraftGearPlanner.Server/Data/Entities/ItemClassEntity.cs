using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarcraftGearPlanner.Server.Data.Entities;

[Index("ClassId", IsUnique = true)]
public class ItemClassEntity : BaseEntity
{
	public int ClassId { get; set; }
	public string? Name { get; set; }
	public int? DisplayOrder { get; set; }

	[InverseProperty(nameof(ItemSubclassEntity.ItemClass))]
	public List<ItemSubclassEntity>? Subclasses { get; set; }

	[InverseProperty(nameof(ItemEntity.ItemClass))]
	public List<ItemEntity>? Items { get; set; }
}
