namespace WarcraftGearPlanner.Server.Models;

public class Spell
{
	[JsonProperty("spell")]
	public IndexReference? Reference { get; set; }
	public string? Description { get; set; }
}
