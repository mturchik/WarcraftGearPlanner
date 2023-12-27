using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
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

		var validateItemsEvents = itemClasses
			.SelectMany(c =>
				c.Subclasses?.Select(s => new ValidateItemsEvent
				{
					ItemClassId = c.Id,
					ItemSubclassId = s.Id,
					ClassId = c.ClassId,
					SubclassId = s.SubclassId,
				}).ToList() ?? new()
			)
			.DistinctBy(e => $"{e.ItemClassId}-{e.ItemSubclassId}")
			.ToList();
		log.LogInformation($"Created {validateItemsEvents.Count} Validate Items Events");

		var messagesSent = 0;
		var messageBatch = await _serviceBusSender.CreateMessageBatchAsync();
		foreach (var validateItemsEvent in validateItemsEvents)
		{
			var message = new ServiceBusMessage(JsonConvert.SerializeObject(validateItemsEvent));
			if (!messageBatch.TryAddMessage(message))
			{
				log.LogInformation($"Sending batch of {messageBatch.Count} messages");

				await _serviceBusSender.SendMessagesAsync(messageBatch);
				messagesSent += messageBatch.Count;
				messageBatch = await _serviceBusSender.CreateMessageBatchAsync();
			}
		}

		if (messageBatch.Count > 0)
		{
			log.LogInformation($"Sending batch of {messageBatch.Count} messages");

			await _serviceBusSender.SendMessagesAsync(messageBatch);
			messagesSent += messageBatch.Count;
		}
		log.LogInformation($"Sent {messagesSent} messages");

		return new OkObjectResult($"Successfully sent {messagesSent} Validate Items Events!");
	}
}
