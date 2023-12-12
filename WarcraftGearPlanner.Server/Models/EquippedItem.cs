namespace WarcraftGearPlanner.Models;

public class EquippedItem
{
	public IndexReference? Item { get; set; }
	public TypeReference? Slot { get; set; }
	public long Quantity { get; set; }
	public TypeReference? Quality { get; set; }
	public string? Name { get; set; }
	public MediaReference? Media { get; set; }
	public TypeReference? Binding { get; set; }
	public ValueReference? Armor { get; set; }
	public ItemRequirements? Requirements { get; set; }
	public ValueReference? Durability { get; set; }
	public WeaponStats? Weapon { get; set; }
	public List<Enchantment> Enchantments { get; set; } = new();
	public List<ValueReference> Stats { get; set; } = new();

	[JsonProperty("item_class")]
	public IndexReference? ItemClass { get; set; }

	[JsonProperty("item_subclass")]
	public IndexReference? ItemSubclass { get; set; }

	[JsonProperty("inventory_type")]
	public TypeReference? InventoryType { get; set; }

	[JsonProperty("sell_price")]
	public SellPrice? SellPrice { get; set; }

	[JsonProperty("is_subclass_hidden", NullValueHandling = NullValueHandling.Ignore)]
	public bool? IsSubclassHidden { get; set; }

	[JsonProperty("unique_equipped", NullValueHandling = NullValueHandling.Ignore)]
	public string? UniqueEquipped { get; set; }
}
