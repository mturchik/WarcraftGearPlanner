namespace WarcraftGearPlanner.Models;

public class Enchantment
{
	[JsonProperty("enchantment_id")]
	public long EnchantmentId { get; set; }

	[JsonProperty("enchantment_slot")]
	public EnchantmentSlot? EnchantmentSlot { get; set; }

	[JsonProperty("display_string")]
	public string? DisplayString { get; set; }

	[JsonProperty("source_item")]
	public IndexReference? SourceItem { get; set; }

}
