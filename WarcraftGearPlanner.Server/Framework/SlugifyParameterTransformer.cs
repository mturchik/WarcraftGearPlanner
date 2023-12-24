using System.Text.RegularExpressions;

internal partial class SlugifyParameterTransformer : IOutboundParameterTransformer
{
	public string? TransformOutbound(object? value)
	{
		var valStr = value?.ToString()?.Trim() ?? "";
		var valSpace = CapitalizedRegex().Replace(valStr, " $1").Trim();
		var valSlug = valSpace?.ToLowerInvariant().Replace(' ', '-');
		return valSlug;
	}

	[GeneratedRegex("([A-Z])", RegexOptions.Compiled)]
	private static partial Regex CapitalizedRegex();
}