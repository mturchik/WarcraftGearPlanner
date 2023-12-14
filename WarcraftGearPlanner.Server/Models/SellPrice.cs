namespace WarcraftGearPlanner.Server.Models;

public class SellPrice
{
	public long Value { get; set; }

	[JsonIgnore]
	public string DisplayString => $"{Currency?.Header} {Currency?.Gold}g {Currency?.Silver}s {Currency?.Copper}c";

	[JsonProperty("display_strings")]
	public CurrencyDescription? Currency { get; set; }
}
