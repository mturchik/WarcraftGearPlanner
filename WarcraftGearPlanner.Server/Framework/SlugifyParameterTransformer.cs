internal class SlugifyParameterTransformer : IOutboundParameterTransformer
{
	public string? TransformOutbound(object? value)
	{
		var str = value?.ToString();
		return str?.ToLowerInvariant().Replace(' ', '-');
	}
}