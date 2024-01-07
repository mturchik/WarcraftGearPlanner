using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarcraftGearPlanner.Server.Data.Entities;

[Index("Type", "Name", IsUnique = true)]
public class ItemQualityEntity : BaseEntity
{
	public string Type { get; set; } = "";
	public string Name { get; set; } = "";
	public int? DisplayOrder { get; set; }
	public string? Color { get; set; }

	[InverseProperty(nameof(ItemEntity.ItemQuality))]
	public List<ItemEntity>? Items { get; set; }
}
