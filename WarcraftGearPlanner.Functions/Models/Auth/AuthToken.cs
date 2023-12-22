namespace WarcraftGearPlanner.Functions.Models.Response;

public class AuthToken
{
	[JsonProperty("access_token")]
	public string? AccessToken { get; set; }
	[JsonProperty("expires_in")]
	public int ExpiresIn { get; set; }
	[JsonIgnore]
	public DateTime? ExpiresOn { get; set; }
}