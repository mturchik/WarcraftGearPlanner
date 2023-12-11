namespace WarcraftGearPlanner.Models;

public class Realm : IndexReference
{
	public string? Locale { get; set; }
	public string? TimeZone { get; set; }
	public string? Slug { get; set; }
}