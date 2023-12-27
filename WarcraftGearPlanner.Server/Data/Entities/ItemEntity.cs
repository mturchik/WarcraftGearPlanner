using System.ComponentModel.DataAnnotations.Schema;

namespace WarcraftGearPlanner.Server.Data.Entities;

public class ItemEntity : BaseEntity
{
	public long ItemId { get; set; }
	public long Level { get; set; }
	public string? Name { get; set; }
	public long RequiredLevel { get; set; }
	public long MaxCount { get; set; }
	public long PurchaseQuantity { get; set; }
	public long PurchasePrice { get; set; }
	public long SellPrice { get; set; }
	public bool IsEquippable { get; set; }
	public bool IsStackable { get; set; }

	[ForeignKey(nameof(ItemClass))]
	public Guid? ItemClassId { get; set; }
	public ItemClassEntity? ItemClass { get; set; }

	[ForeignKey(nameof(ItemSubclass))]
	public Guid? ItemSubclassId { get; set; }
	public ItemSubclassEntity? ItemSubclass { get; set; }

	[ForeignKey(nameof(ItemQuality))]
	public Guid? ItemQualityId { get; set; }
	public ItemQualityEntity? ItemQuality { get; set; }

	[ForeignKey(nameof(InventoryType))]
	public Guid? InventoryTypeId { get; set; }
	public InventoryTypeEntity? InventoryType { get; set; }
}
