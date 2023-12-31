using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace WarcraftGearPlanner.Functions.Functions;

public class Status
{
	[FunctionName("Status")]
	public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, ILogger log)
	{
		if (req is null) throw new ArgumentNullException(nameof(req));

		log.LogInformation("Status processed a request.");

		var version = Assembly.GetExecutingAssembly().GetName().Version;
		return new OkObjectResult($"APPLICATION RUNNING - v{version}");
	}
}
