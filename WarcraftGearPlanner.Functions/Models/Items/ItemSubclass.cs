namespace WarcraftGearPlanner.Functions.Models.Items;
public class ItemSubclass
{
	[JsonProperty("subclass_id")]
	public int Id { get; set; }
	[JsonProperty("class_id")]
	public int ClassId { get; set; }
	[JsonProperty("display_name")]
	public string? Name { get; set; }
	[JsonProperty("verbose_name")]
	public string? VerboseName { get; set; }
	[JsonProperty("hide_subclass_in_tooltips")]
	public bool? HideTooltip { get; set; }
}
