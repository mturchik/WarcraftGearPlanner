using WarcraftGearPlanner.Functions.Models.Shared;

namespace WarcraftGearPlanner.Functions.Models.Items;

public class ItemClassIndex
{
	[JsonProperty("item_classes")]
	public List<IndexReference> ItemClasses { get; set; } = new();
}
