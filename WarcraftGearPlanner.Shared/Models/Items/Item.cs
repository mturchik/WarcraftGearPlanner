namespace WarcraftGearPlanner.Shared.Models.Items;
public class Item : BaseModel
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

	public Guid ItemClassId { get; set; }
	public int? ItemClassClassId { get; set; }
	public string? ItemClassName { get; set; }

	public Guid ItemSubclassId { get; set; }
	public int? ItemSubclassSubclassId { get; set; }
	public string? ItemSubclassName { get; set; }

	public Guid ItemQualityId { get; set; }
	public string? ItemQualityType { get; set; }
	public string? ItemQualityName { get; set; }

	public Guid InventoryTypeId { get; set; }
	public string? InventoryTypeType { get; set; }
	public string? InventoryTypeName { get; set; }
}
