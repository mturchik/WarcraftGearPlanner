using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WarcraftGearPlanner.Functions.Functions;
using WarcraftGearPlanner.Tests;

namespace WarcraftGearPlanner.Functions.Tests.Functions;
[TestClass]
public class StatusTest : BaseTest<Status>
{
	[TestMethod]
	public void Run()
	{
		// Arrange
		var request = Mocker.GetMock<HttpRequest>();
		var logger = Mocker.GetMock<ILogger>();

		// Act
		var result = Instance.Run(request.Object, logger.Object);

		// Assert
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType(result, typeof(OkObjectResult));
		Assert.IsTrue(((OkObjectResult)result).Value?.ToString()?.Contains("APPLICATION RUNNING"));
	}
}
