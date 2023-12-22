using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;

namespace WarcraftGearPlanner.Functions.Extensions;

public static class HttpClientExtensions
{
	private static readonly bool WriteDebug = false;

	public static async Task<T?> Send<T>(this HttpClient client, HttpRequestMessage request)
	{
		var response = await client.SendAsync(request);

		var obj = await response.ConvertResponse<T>();

		return obj;
	}

	public static async Task<T?> ConvertResponse<T>(this HttpResponseMessage response)
	{
		if (WriteDebug) await WriteToDebugFile(response);

		if (response is null || !response.IsSuccessStatusCode || response.Content is null)
			return default;

		var content = await response.Content.ReadAsStringAsync();

		if (string.IsNullOrEmpty(content)) return default;

		var obj = JsonConvert.DeserializeObject<T>(content);

		return obj;
	}

	private static async Task WriteToDebugFile(HttpResponseMessage response)
	{
		var directory = SpecialDirectories.MyDocuments + @"\WarcraftGearPlanner_DebugResponses";
		Directory.CreateDirectory(directory);
		using var writer = new StreamWriter($"{directory}/{DateTime.Now:yyMMdd-HHmmss_ff}.json", true);
		await writer.WriteLineAsync($"// Request URI: {response.RequestMessage?.RequestUri}");
		await writer.WriteLineAsync($"// Status Code: {response.StatusCode}");
		foreach (var header in response.Headers)
			await writer.WriteLineAsync($"// Headers: {header.Key}: {string.Join(",", header.Value)}");

		var content = await response.Content.ReadAsStringAsync();
		if (string.IsNullOrEmpty(content)) await writer.WriteLineAsync("// Content: null");
		else
		{
			var obj = JObject.Parse(content);
			var formattedContent = JsonConvert.SerializeObject(obj, Formatting.Indented);
			await writer.WriteLineAsync(formattedContent);
		}
	}
}