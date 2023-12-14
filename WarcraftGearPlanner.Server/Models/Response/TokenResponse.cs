namespace WarcraftGearPlanner.Server.Models.Response;

public class TokenResponse
{
	public string? Access_Token { get; set; }
	public string? Token_Type { get; set; }
	public int Expires_In { get; set; }
	public string? Scope { get; set; }
	[JsonIgnore]
	public DateTime? ExpiresOn { get; set; }
}