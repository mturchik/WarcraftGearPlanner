using WarcraftGearPlanner.Functions.Models.Shared;

namespace WarcraftGearPlanner.Functions.Models.Items;
public class ItemSearchResult
{
	public long Id { get; set; }
	public long Level { get; set; }
	public Localization? Name { get; set; }

	[JsonProperty("required_level")]
	public long RequiredLevel { get; set; }

	[JsonProperty("max_count")]
	public long MaxCount { get; set; }

	[JsonProperty("purchase_quantity")]
	public long PurchaseQuantity { get; set; }

	[JsonProperty("purchase_price")]
	public long PurchasePrice { get; set; }

	[JsonProperty("sell_price")]
	public long SellPrice { get; set; }

	[JsonProperty("is_equippable")]
	public bool IsEquippable { get; set; }

	[JsonProperty("is_stackable")]
	public bool IsStackable { get; set; }

	[JsonProperty("item_subclass")]
	public LocalizationReference? ItemSubclass { get; set; }

	[JsonProperty("item_class")]
	public LocalizationReference? ItemClass { get; set; }

	[JsonProperty("quality")]
	public LocalizationReference? Quality { get; set; }

	[JsonProperty("inventory_type")]
	public LocalizationReference? InventoryType { get; set; }
}