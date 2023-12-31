using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using WarcraftGearPlanner.Server.Data.Entities;

namespace WarcraftGearPlanner.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StatusController(IMemoryCache memoryCache) : ControllerBase
{
	private readonly IMemoryCache memoryCache = memoryCache;

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<string> GetStatus()
	{
		var version = GetType().Assembly.GetName().Version;
		return Ok($"APPLICATION RUNNING - v{version}");
	}

	[HttpGet]
	[Route("cache")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<string> GetCacheStatus()
	{
		var cacheStats = memoryCache.GetCurrentStatistics();
		var sb = new StringBuilder();
		sb.AppendLine($"Cache Stats:");
		if (cacheStats is null) sb.AppendLine("Cache Stats are null!");
		else
		{
			sb.AppendLine($"CurrentEntryCount: {cacheStats.CurrentEntryCount}");
			sb.AppendLine($"TotalMisses: {cacheStats.TotalMisses}");
			sb.AppendLine($"TotalHits: {cacheStats.TotalHits}");
		}
		return Ok(sb.ToString());
	}

	[HttpPost]
	[Route("cache/reset")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<string> ResetCache()
	{
		var entities = typeof(BaseEntity).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseEntity)));

		foreach (var entity in entities) memoryCache.Remove(entity.Name);

		return Ok("Cache Reset!");
	}
}
