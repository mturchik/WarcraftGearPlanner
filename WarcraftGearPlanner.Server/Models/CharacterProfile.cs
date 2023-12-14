namespace WarcraftGearPlanner.Server.Models;

public class CharacterProfile
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public int Level { get; set; }
	public int Experience { get; set; }
	[JsonProperty("is_ghost")]
	public bool IsGhost { get; set; }
	[JsonIgnore] public bool IsAlive => !IsGhost;
	[JsonProperty("average_item_level")]
	public int AverageItemLevel { get; set; }
	[JsonProperty("equipped_item_level")]
	public int EquippedItemLevel { get; set; }
	[JsonProperty("last_login_timestamp")]
	public long LastLoginTimestamp { get; set; }
	[JsonIgnore] public DateTimeOffset LastLogin => DateTimeOffset.FromUnixTimeMilliseconds(LastLoginTimestamp).ToLocalTime();
	public IndexReference? Gender { get; set; }
	public IndexReference? Faction { get; set; }
	public IndexReference? Race { get; set; }
	[JsonProperty("character_class")]
	public IndexReference? CharacterClass { get; set; }
	public Realm? Realm { get; set; }
	public Guild? Guild { get; set; }

}
