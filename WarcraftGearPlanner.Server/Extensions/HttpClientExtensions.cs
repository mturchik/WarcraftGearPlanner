using Newtonsoft.Json.Linq;

namespace WarcraftGearPlanner.Extensions;

public static class HttpClientExtensions
{
	private static readonly bool WriteDebug = false;

	public static async Task<T?> Send<T>(this HttpClient client, HttpRequestMessage request)
	{
		var response = await client.SendAsync(request);

		return await response.ConvertResponse<T>();
	}

	public static async Task<T?> ConvertResponse<T>(this HttpResponseMessage response)
	{
		if (WriteDebug) await WriteToDebugFile(response);

		if (response is null || !response.IsSuccessStatusCode || response.Content is null)
			return default;

		var content = await response.Content.ReadAsStringAsync();

		if (string.IsNullOrEmpty(content)) return default;

		return JsonConvert.DeserializeObject<T>(content);
	}

	private static async Task WriteToDebugFile(HttpResponseMessage response)
	{
		using var writer = new StreamWriter($"DebugApiResponses/{DateTime.Now:yyMMdd-HHmmss_ff}.json", true);
		await writer.WriteLineAsync($"// Request URI: {response.RequestMessage?.RequestUri}");
		await writer.WriteLineAsync($"// Status Code: {response.StatusCode}");
		foreach (var header in response.Headers)
			await writer.WriteLineAsync($"// Headers: {header.Key}: {string.Join(",", header.Value)}");

		var content = await response.Content.ReadAsStringAsync();
		if (string.IsNullOrEmpty(content)) await writer.WriteLineAsync("// Content: null");
		else
		{
			var formattedContent = JToken.Parse(content).ToString(Formatting.Indented);
			await writer.WriteLineAsync(formattedContent);
		}
	}
}