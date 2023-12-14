namespace WarcraftGearPlanner.Server.Models;

public class ItemSetEffect
{
	[JsonProperty("display_string")]
	public string? DisplayString { get; set; }
	[JsonProperty("required_count")]
	public long RequiredCount { get; set; }
	[JsonProperty("isActive")]
	public bool? IsActive { get; set; }
}
