using WarcraftGearPlanner.Functions.Models.Shared;

namespace WarcraftGearPlanner.Functions.Models.Items;

public class ItemClass
{
	[JsonProperty("class_id")]
	public int Id { get; set; }
	public string? Name { get; set; }
	[JsonProperty("item_subclasses")]
	public List<IndexReference> Subclasses { get; set; } = new();
}
