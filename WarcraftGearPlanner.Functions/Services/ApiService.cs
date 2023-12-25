using Azure.Core;
using Azure.Identity;
using System.Net.Http.Headers;
using System.Text;
using WarcraftGearPlanner.Functions.Extensions;

namespace WarcraftGearPlanner.Functions.Services;
public class ApiService : IApiService
{
	private readonly HttpClient _httpClient;
	private readonly string _apiUrl;

	public ApiService(IHttpClientFactory httpClientFactory)
	{
		_httpClient = httpClientFactory.CreateClient("ApiService");
		_apiUrl = Environment.GetEnvironmentVariable("API_URL")
			?? throw new ApplicationException("API_URL environment variable is not set");

		var auth = new EnvironmentCredential();
		var clientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID")
			?? throw new ApplicationException("AZURE_CLIENT_ID environment variable is not set");
		var accessToken = auth.GetToken(new TokenRequestContext(new[] { $"api://{clientId}/.default" }));
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Token);
	}

	public Task Delete(string url) => _httpClient.DeleteAsync(_apiUrl + url);

	public async Task<T?> Get<T>(string url)
	{
		var response = await _httpClient.GetAsync(_apiUrl + url);
		var obj = await response.ConvertResponse<T>();
		return obj;
	}

	public async Task<T?> Post<T>(string url, T? body)
	{
		var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(_apiUrl + url, content);
		var obj = await response.ConvertResponse<T>();
		return obj;
	}

	public async Task<T?> Put<T>(string url, T? body)
	{
		var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
		var response = await _httpClient.PutAsync(_apiUrl + url, content);
		var obj = await response.ConvertResponse<T>();
		return obj;
	}

}
