namespace WarcraftGearPlanner.Server.Models;

public class DamageStats
{
	[JsonProperty("min_value")]
	public long MinValue { get; set; }

	[JsonProperty("max_value")]
	public long MaxValue { get; set; }

	[JsonProperty("display_string")]
	public string? DisplayString { get; set; }

	[JsonProperty("damage_class")]
	public IndexReference? DamageClass { get; set; }
}
