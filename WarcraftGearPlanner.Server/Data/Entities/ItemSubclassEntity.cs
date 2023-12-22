using System.ComponentModel.DataAnnotations.Schema;

namespace WarcraftGearPlanner.Server.Data.Entities;

public class ItemSubclassEntity : BaseEntity
{
	public int SubclassId { get; set; }
	public string? Name { get; set; }

	[ForeignKey(nameof(ItemClass))]
	public Guid ItemClassId { get; set; }
	[JsonIgnore]
	public ItemClassEntity? ItemClass { get; set; }
}
