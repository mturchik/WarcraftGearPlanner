using System.ComponentModel.DataAnnotations.Schema;

namespace WarcraftGearPlanner.Server.Data.Entities;

public class ItemQualityEntity : BaseEntity
{
	public string Type { get; set; } = "";
	public string Name { get; set; } = "";

	[InverseProperty(nameof(ItemEntity.ItemQuality))]
	public List<ItemEntity>? Items { get; set; }
}
