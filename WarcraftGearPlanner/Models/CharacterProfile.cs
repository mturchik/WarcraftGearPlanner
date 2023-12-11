namespace WarcraftGearPlanner.Models;

public class CharacterProfile
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public int Level { get; set; }
	public int Experience { get; set; }
	public int Average_Item_Level { get; set; }
	public int Equipped_Item_Level { get; set; }
	public long Achievement_Points { get; set; }
	public long Last_Login_Timestamp { get; set; }
	public TypeReference? Gender { get; set; }
	public TypeReference? Faction { get; set; }
	public IndexReference? Race { get; set; }
	public IndexReference? Character_Class { get; set; }
	public Realm? Realm { get; set; }
	public Guild? Guild { get; set; }

	[JsonIgnore]
	public DateTimeOffset LastLogin => DateTimeOffset.FromUnixTimeMilliseconds(Last_Login_Timestamp).ToLocalTime();
}
