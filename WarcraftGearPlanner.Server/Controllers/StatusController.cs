using Microsoft.AspNetCore.Mvc;

namespace WarcraftGearPlanner.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StatusController(IConfiguration configuration) : ControllerBase
{
	private readonly IConfiguration configuration = configuration;

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<string> GetStatus()
	{
		var version = GetType().Assembly.GetName().Version;
		var configConnString = configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
		var envConnString = Environment.GetEnvironmentVariable("wgp-sql-db-conn-string");
		var envConnString2 = configuration.GetValue<string>("wgp-sql-db-conn-string");
		return Ok($"APPLICATION RUNNING - v{version} " +
			$"- configConnString: {string.IsNullOrEmpty(configConnString)} " +
			$"- envConnString: {string.IsNullOrEmpty(envConnString)} " +
			$"- envConnString2: {string.IsNullOrEmpty(envConnString2)}");
	}
}
