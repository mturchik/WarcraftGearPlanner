namespace WarcraftGearPlanner.Models;

public class Skill
{
	public IndexReference? Profession { get; set; }
	public long Level { get; set; }

	[JsonProperty("display_string")]
	public string? DisplayString { get; set; }
}
