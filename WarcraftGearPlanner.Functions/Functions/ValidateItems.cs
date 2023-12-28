using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using WarcraftGearPlanner.Functions.Extensions;
using WarcraftGearPlanner.Functions.Models.Events;
using WarcraftGearPlanner.Functions.Services;
using ItemClassDTO = WarcraftGearPlanner.Shared.Models.Items.ItemClass;

namespace WarcraftGearPlanner.Functions.Functions;

public class ValidateItems
{
	private readonly IApiService _apiService;
	private readonly ServiceBusSender _serviceBusSender;

	public ValidateItems(IApiService apiService, ServiceBusClient _serviceBusClient)
	{
		_apiService = apiService;
		var queueName = Environment.GetEnvironmentVariable("SERVICE_BUS_QUEUE_VALIDATE_ITEMS");
		_serviceBusSender = _serviceBusClient.CreateSender(queueName);
	}

	[FunctionName("ValidateItems")]
	public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, ILogger log)
	{
		if (req is null) throw new ArgumentNullException(nameof(req));

		log.LogInformation("ValidateItems processed a request.");

		var itemClasses = await _apiService.Get<List<ItemClassDTO>>("/item-classes") ?? new();
		log.LogInformation($"Retrieved {itemClasses.Count} item classes from API");

		List<ValidateItemsEvent> validateItemsEvents = new();
		foreach (var itemClass in itemClasses)
			foreach (var itemSubclass in itemClass.Subclasses ?? new())
				validateItemsEvents.Add(new()
				{
					ItemClassId = itemClass.Id,
					ClassId = itemClass.ClassId,
					ItemSubclassId = itemSubclass.Id,
					SubclassId = itemSubclass.SubclassId,
				});
		log.LogInformation($"Created {validateItemsEvents.Count} Validate Items Events");

		var messagesSent = await _serviceBusSender.SendEventsAsync(validateItemsEvents);
		log.LogInformation($"Sent {messagesSent} messages");

		return new OkObjectResult($"Successfully sent {messagesSent} Validate Items Events!");
	}
}
