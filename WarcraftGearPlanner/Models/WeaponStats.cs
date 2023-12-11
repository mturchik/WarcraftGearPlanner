namespace WarcraftGearPlanner.Models;

public class WeaponStats
{
	public DamageStats? Damage { get; set; }
	public ValueReference? Dps { get; set; }

	[JsonProperty("attack_speed")]
	public ValueReference? AttackSpeed { get; set; }
}
