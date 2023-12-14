using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text;
using WarcraftGearPlanner.Server.Extensions;
using WarcraftGearPlanner.Server.Models;
using WarcraftGearPlanner.Server.Models.Response;

namespace WarcraftGearPlanner.Server.Services;

public class BattleNetService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache) : IBattleNetService
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("BattleNetService");
	private readonly IConfiguration _configuration = configuration;
	private readonly IMemoryCache _memoryCache = memoryCache;
	private readonly string Base = "https://us.api.blizzard.com";

	private enum Namespace
	{
		Static,
		Dynamic,
		Profile
	}

	#region Utility

	private async Task SetAuthorizeToken(HttpRequestMessage requestMessage)
	{
		var accessToken = await GetAuthorizationToken();
		requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
	}

	private async Task<string?> GetAuthorizationToken()
	{
		var token = await _memoryCache.GetOrCreateAsync("BattleNetToken", entry => SendTokenRequest());

		if (token?.ExpiresOn is null || token.ExpiresOn <= DateTime.UtcNow)
		{
			token = await SendTokenRequest();
			if (token != null) _memoryCache.Set("BattleNetToken", token);
		}

		return token?.Access_Token;
	}

	private async Task<TokenResponse?> SendTokenRequest()
	{
		var tokenRequest = CreateTokenRequest();
		var token = await _httpClient.Send<TokenResponse>(tokenRequest);
		if (token is null) return null;

		// Set expires timestamp to keep an active OAuth token
		token.ExpiresOn = DateTime.UtcNow.AddSeconds(token.Expires_In).Subtract(TimeSpan.FromHours(4));
		return token;
	}

	private HttpRequestMessage CreateTokenRequest()
	{
		HttpRequestMessage tokenRequest = new(HttpMethod.Post, "https://oauth.battle.net/token");

		var clientId = _configuration.GetValue<string>("BattleNet:ClientId");
		var clientSecret = _configuration.GetValue<string>("BattleNet:ClientSecret");
		var byteArray = new UTF8Encoding().GetBytes($"{clientId}:{clientSecret}");
		tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

		List<KeyValuePair<string, string>> formData =
		[
			new(":region", "us"),
			new("grant_type", "client_credentials")
		];
		tokenRequest.Content = new FormUrlEncodedContent(formData);

		return tokenRequest;
	}

	private static string SetLocale(string url)
	{
		if (url.Contains("locale=")) return url;

		var qualifier = url.Contains('?') ? "&" : "?";
		return url + qualifier + "locale=en_US";
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

	private async Task<T?> Get<T>(string url, Namespace bnetNamespace)
	{
		url = SetLocale(url);
		var request = new HttpRequestMessage(HttpMethod.Get, url);
		SetNamespace(request, bnetNamespace);
		await SetAuthorizeToken(request);
		return await _httpClient.Send<T>(request); ;
	}

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
	public Task<RealmIndexResponse?> GetRealmIndex() => Get<RealmIndexResponse>($"{Base}/data/wow/realm/index", Namespace.Dynamic);

	public Task<CharacterProfile?> GetCharacterProfile(string realmSlug, string characterName) => Get<CharacterProfile>($"{Base}/profile/wow/character/{realmSlug}/{characterName.ToLower()}", Namespace.Profile);
	public Task<EquipmentSummary?> GetEquipmentSummary(string realmSlug, string characterName) => Get<EquipmentSummary>($"{Base}/profile/wow/character/{realmSlug}/{characterName.ToLower()}/equipment", Namespace.Profile);

	public Task<MediaReference?> GetItemMedia(int itemId) => Get<MediaReference>($"{Base}/data/wow/media/item/{itemId}", Namespace.Static);

	public async Task<IEnumerable<MediaReference>> GetItemMedia(int[] itemIds)
	{
		var getMediaTasks = itemIds.Select(GetItemMedia);
		var mediaReferences = await Task.WhenAll(getMediaTasks);
		return mediaReferences?.Where(m => m != null).Select(m => m!)
			?? Enumerable.Empty<MediaReference>();
	}
}