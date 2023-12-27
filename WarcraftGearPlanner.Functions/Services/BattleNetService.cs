using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using WarcraftGearPlanner.Functions.Extensions;
using WarcraftGearPlanner.Functions.Models.Auth;
using WarcraftGearPlanner.Functions.Models.Enum;
using WarcraftGearPlanner.Functions.Models.Items;
using WarcraftGearPlanner.Functions.Models.Realms;
using WarcraftGearPlanner.Functions.Models.Search;

namespace WarcraftGearPlanner.Functions.Services;

public class BattleNetService : IBattleNetService
{
	private readonly HttpClient _httpClient;
	private readonly IMemoryCache _memoryCache;

	public BattleNetService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
	{
		_httpClient = httpClientFactory.CreateClient("BattleNetService");
		_memoryCache = memoryCache;
	}

	#region Utility

	#region Token

	private async Task SetAuthorizeToken(HttpRequestMessage requestMessage)
	{
		var accessToken = await GetAuthorizationToken();
		requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
	}

	private async Task<string?> GetAuthorizationToken()
	{
		var accessToken = await _memoryCache.GetOrCreateAsync("BattleNetAccessToken", async entry =>
		{
			entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
			return await SendTokenRequest();
		});
		if (accessToken?.ExpiresOn is null || accessToken.ExpiresOn <= DateTime.UtcNow)
		{
			accessToken = await SendTokenRequest();
			if (accessToken == null) throw new AuthenticationException("Unable to retrieve Battle.net OAuth token");
		}

		return accessToken.AccessToken;
	}

	private async Task<AuthToken?> SendTokenRequest()
	{
		var tokenRequest = CreateTokenRequest();
		var token = await _httpClient.Send<AuthToken>(tokenRequest);
		if (token is null) return null;

		// Set expires timestamp to keep an active OAuth token
		token.ExpiresOn = DateTime.UtcNow.AddSeconds(token.ExpiresIn).Subtract(TimeSpan.FromHours(1));
		return token;
	}

	private static HttpRequestMessage CreateTokenRequest()
	{
		var authUrl = Environment.GetEnvironmentVariable("BATTLENET_AUTHURL");
		HttpRequestMessage tokenRequest = new(HttpMethod.Post, authUrl);

		var clientId = Environment.GetEnvironmentVariable("BATTLENET_CLIENTID");
		var clientSecret = Environment.GetEnvironmentVariable("BATTLENET_CLIENTSECRET");
		var byteArray = new UTF8Encoding().GetBytes($"{clientId}:{clientSecret}");
		tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

		var formData = new List<KeyValuePair<string, string>>()
		{
			new(":region", "us"),
			new("grant_type", "client_credentials")
		};
		tokenRequest.Content = new FormUrlEncodedContent(formData);

		return tokenRequest;
	}

	#endregion Token

	#region Get

	private async Task<T?> Get<T>(string route, Namespace bnetNamespace)
	{
		var url = GetUrl(route);
		var request = new HttpRequestMessage(HttpMethod.Get, url);
		SetNamespace(request, bnetNamespace);
		await SetAuthorizeToken(request);
		return await _httpClient.Send<T>(request);
	}

	private static string GetUrl(string route)
	{
		var url = Environment.GetEnvironmentVariable("BATTLENET_BASEURL");
		url += route;

		if (!url.Contains("locale="))
		{
			var qualifier = url.Contains('?') ? "&" : "?";
			url += qualifier + "locale=en_US";
		}

		return url;
	}

	private static void SetNamespace(HttpRequestMessage requestMessage, Namespace bnetNamespace)
	{
		switch (bnetNamespace)
		{
			case Namespace.Static:
				requestMessage.Headers.Add("Battlenet-Namespace", "static-classic1x-us");
				break;
			case Namespace.Dynamic:
				requestMessage.Headers.Add("Battlenet-Namespace", "dynamic-classic1x-us");
				break;
			case Namespace.Profile:
				requestMessage.Headers.Add("Battlenet-Namespace", "profile-classic1x-us");
				break;
		}
	}

	#endregion Get

	#endregion Utility

	/**
		//Account Information:
		///profile/user/wow

		//Character Information:
		///character
		///protected-character
		///character/status
		///character/equipment
		///character/pvp-summary (New Mak’gora kill-count added here)
		///character/pvp-bracket
		///character/statistics
		///character/hunter-pets

		//Guild Information:
		///guild
		///guild/roster
		///guild/activity
		///guild/achievements
	*/
	public Task<RealmIndex?> GetRealmIndex() => Get<RealmIndex>($"/data/wow/realm/index", Namespace.Dynamic);

	//public Task<CharacterProfile?> GetCharacterProfile(string realmSlug, string characterName)
	//	=> Get<CharacterProfile>($"{Base}/profile/wow/character/{realmSlug}/{characterName.ToLower()}", Namespace.Profile);
	//public Task<EquipmentSummary?> GetEquipmentSummary(string realmSlug, string characterName)
	//	=> Get<EquipmentSummary>($"{Base}/profile/wow/character/{realmSlug}/{characterName.ToLower()}/equipment", Namespace.Profile);

	//public Task<Item?> GetItem(int itemId)
	//	=> Get<Item>($"{Base}/data/wow/item/{itemId}", Namespace.Static);
	//public Task<Media?> GetItemMedia(int itemId)
	//	=> Get<Media>($"{Base}/data/wow/media/item/{itemId}", Namespace.Static);
	public Task<ItemClassIndex?> GetItemClassIndex() => Get<ItemClassIndex>($"/data/wow/item-class/index", Namespace.Static);
	public Task<ItemClass?> GetItemClass(int itemClassId) => Get<ItemClass>($"/data/wow/item-class/{itemClassId}", Namespace.Static);
	public Task<ItemSubclass?> GetItemSubclass(int itemClassId, int itemSubclassId) => Get<ItemSubclass>($"/data/wow/item-class/{itemClassId}/item-subclass/{itemSubclassId}", Namespace.Static);
	public Task<SearchResponse<ItemSearchResult>?> SearchItems(SearchRequest<ItemSearchParameters> request)
		=> Get<SearchResponse<ItemSearchResult>>($"/data/wow/search/item{request.GetQueryString()}", Namespace.Static);

	//public async Task<IEnumerable<Media>> GetItemMedia(int[] itemIds)
	//{
	//	var getMediaTasks = itemIds.Select(GetItemMedia);
	//	var mediaReferences = await Task.WhenAll(getMediaTasks);
	//	return mediaReferences?.Where(m => m != null).Select(m => m!)
	//		?? Enumerable.Empty<Media>();
	//}
}