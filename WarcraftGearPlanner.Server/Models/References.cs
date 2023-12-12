namespace WarcraftGearPlanner.Models;

public class IndexReference
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public UrlReference? Key { get; set; }
}

public class UrlReference
{
	public string? Href { get; set; }
}

public class TypeReference
{
	public string? Type { get; set; }
	public string? Name { get; set; }
}

public class ValueReference
{
	public TypeReference? Type { get; set; }
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
	public List<AssetReference> Assets { get; set; } = new();

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