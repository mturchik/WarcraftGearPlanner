namespace WarcraftGearPlanner.Models;

public class Guild : IndexReference
{
	public Realm? Realm { get; set; }
	public TypeReference? Faction { get; set; }
}
