namespace WarcraftGearPlanner.Models;

public class EquipmentSummary
{
	public CharacterProfile? Character { get; set; }

	[JsonProperty("equipped_items")]
	public List<EquippedItem> EquippedItems { get; set; } = new();

	//[JsonProperty("equipped_item_sets")]
	//public List<EquippedItemSet> EquippedItemSets { get; set; }
}
