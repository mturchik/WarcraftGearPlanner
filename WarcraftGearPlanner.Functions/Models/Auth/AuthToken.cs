namespace WarcraftGearPlanner.Functions.Models.Auth;

public class AuthToken
{
	[JsonProperty("access_token")]
	public string? AccessToken { get; set; }
	[JsonProperty("expires_in")]
	public int ExpiresIn { get; set; }
	[JsonIgnore]
	public DateTime? ExpiresOn { get; set; }
}