namespace WarcraftGearPlanner.Server.Models;

public class Guild : IndexReference
{
	public Realm? Realm { get; set; }
	public IndexReference? Faction { get; set; }
}
