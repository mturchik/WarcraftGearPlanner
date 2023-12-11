using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using WarcraftGearPlanner.Extensions;
using WarcraftGearPlanner.Models;
using WarcraftGearPlanner.Models.Response;

namespace WarcraftGearPlanner.Services;

public class BattleNetService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IBattleNetService
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("BattleNetService");
	private readonly IConfiguration _configuration = configuration;
	private readonly string Base = "https://us.api.blizzard.com";

	private enum Namespace
	{
		Static,
		Dynamic,
		Profile
	}

	private TokenResponse? Token { get; set; }

	#region Utility

	private async Task SetAuthorizeToken(HttpRequestMessage requestMessage)
	{
		if (Token?.ExpiresOn is null || Token.ExpiresOn <= DateTime.UtcNow)
		{
			var tokenRequest = CreateTokenRequest();
			var token = await _httpClient.Send<TokenResponse>(tokenRequest);
			if (token is null) return;

			// Set expires timestamp to keep an active OAuth token
			token.ExpiresOn = DateTime.UtcNow.AddSeconds(token.Expires_In).Subtract(TimeSpan.FromHours(4));
			Token = token;
		}

		requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.Access_Token);
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
    public static readonly string Token = ;

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
	public Task<RealmIndexResponse?> GetRealms() => Get<RealmIndexResponse>($"{Base}/data/wow/realm/index", Namespace.Dynamic);

	public Task<CharacterProfile?> GetCharacterProfile(string name, string realm) => Get<CharacterProfile>($"{Base}/profile/wow/character/{realm}/{name?.ToLower()}", Namespace.Profile);
	public Task<EquipmentSummary?> GetEquipmentSummary(string name, string realm) => Get<EquipmentSummary>($"{Base}/profile/wow/character/{realm}/{name?.ToLower()}/equipment", Namespace.Profile);

	public Task<MediaReference?> GetItemMedia(int id) => Get<MediaReference>($"{Base}/data/wow/media/item/{id}", Namespace.Static);
}