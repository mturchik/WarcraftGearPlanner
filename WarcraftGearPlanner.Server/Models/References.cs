namespace WarcraftGearPlanner.Server.Models;

public class IndexReference
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public int? Id { get; set; }
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string? Type { get; set; }
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string? Name { get; set; }
}

public class ValueReference
{
	public IndexReference? Type { get; set; }
	public decimal Value { get; set; }
	public DisplayReference? Display { get; set; }

	[JsonProperty("display_string")]
	public string? DisplayString { get; set; }
}

public class DisplayReference
{
	public ColorReference? Color { get; set; }

	[JsonProperty("display_string")]
	public string? DisplayString { get; set; }
}

public class ColorReference
{
	[JsonProperty("r")]
	public int Red { get; set; }
	[JsonProperty("g")]
	public int Green { get; set; }
	[JsonProperty("b")]
	public int Blue { get; set; }
	[JsonProperty("a")]
	public decimal Alpha { get; set; }
}

public class MediaReference
{
	public int Id { get; set; }
	public List<AssetReference> Assets { get; set; } = [];

	[JsonIgnore]
	public Uri? DefaultAsset => Assets?.FirstOrDefault()?.Value;
}

public class AssetReference
{
	public string? Key { get; set; }
	public Uri? Value { get; set; }

	[JsonProperty("file_data_id")]
	public long FileDataId { get; set; }
}