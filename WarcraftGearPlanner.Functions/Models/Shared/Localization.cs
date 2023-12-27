namespace WarcraftGearPlanner.Functions.Models.Shared;

public class Localization
{
	[JsonProperty("it_IT")]
	public string? Italian { get; set; }
	[JsonProperty("ru_RU")]
	public string? Russian { get; set; }
	[JsonProperty("en_GB")]
	public string? British { get; set; }
	[JsonProperty("zh_TW")]
	public string? Taiwanese { get; set; }
	[JsonProperty("ko_KR")]
	public string? Korean { get; set; }
	[JsonProperty("en_US")]
	public string? American { get; set; }
	[JsonProperty("es_MX")]
	public string? Mexican { get; set; }
	[JsonProperty("pt_BR")]
	public string? Portuguese { get; set; }
	[JsonProperty("es_ES")]
	public string? Spanish { get; set; }
	[JsonProperty("zh_CN")]
	public string? Chinese { get; set; }
	[JsonProperty("fr_FR")]
	public string? French { get; set; }
	[JsonProperty("de_DE")]
	public string? German { get; set; }
}
