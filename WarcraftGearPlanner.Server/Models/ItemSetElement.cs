namespace WarcraftGearPlanner.Server.Models;

public class ItemSetElement
{
	public IndexReference? Item { get; set; }
	[JsonProperty("is_equipped")]
	public bool? IsEquipped { get; set; }
}
