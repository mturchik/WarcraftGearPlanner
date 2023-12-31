using System.ComponentModel.DataAnnotations.Schema;

namespace WarcraftGearPlanner.Server.Data.Entities;

public class ItemSubclassInventoryTypeEntity : BaseEntity
{
	[ForeignKey(nameof(ItemSubclass))]
	public Guid ItemSubclassId { get; set; }
	public ItemSubclassEntity? ItemSubclass { get; set; }

	[ForeignKey(nameof(InventoryType))]
	public Guid InventoryTypeId { get; set; }
	public InventoryTypeEntity? InventoryType { get; set; }
}
