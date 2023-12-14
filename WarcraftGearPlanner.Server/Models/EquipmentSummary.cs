namespace WarcraftGearPlanner.Server.Models;

public class EquipmentSummary
{
	[JsonProperty("equipped_items")]
	public List<EquippedItem> EquippedItems { get; set; } = [];

	[JsonProperty("equipped_item_sets")]
	public List<EquippedItemSet> EquippedItemSets { get; set; } = [];
}
