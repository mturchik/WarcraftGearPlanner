namespace WarcraftGearPlanner.Server.Models;

public class EquippedItemSet
{
	[JsonProperty("item_set")]
	public IndexReference? ItemSet { get; set; }
	public List<ItemSetElement> Items { get; set; } = [];
	public List<ItemSetEffect> Effects { get; set; } = [];
	[JsonProperty("display_string")]
	public string? DisplayString { get; set; }
}
