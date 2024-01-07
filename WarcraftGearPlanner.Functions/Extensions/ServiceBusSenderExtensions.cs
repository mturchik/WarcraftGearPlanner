using Azure.Messaging.ServiceBus;
using WarcraftGearPlanner.Functions.Models.Events;

namespace WarcraftGearPlanner.Functions.Extensions;
public static class ServiceBusSenderExtensions
{
	public static async Task SendEventAsync<T>(this ServiceBusSender sender, T eventData)
		where T : IEventData
	{
		var message = new ServiceBusMessage(JsonConvert.SerializeObject(eventData))
		{
			ScheduledEnqueueTime = DateTime.UtcNow.AddMinutes(1)
		};
		await sender.SendMessageAsync(message);
	}

	public static async Task<int> SendEventsAsync<T>(this ServiceBusSender sender, IEnumerable<T> eventData)
		where T : IEventData
	{
		var messagesSent = 0;
		var messageBatch = await sender.CreateMessageBatchAsync();

		foreach (var e in eventData)
		{
			var message = new ServiceBusMessage(JsonConvert.SerializeObject(e))
			{
				ScheduledEnqueueTime = DateTime.UtcNow.AddMinutes(2)
			};
			if (!messageBatch.TryAddMessage(message))
			{
				await sender.SendMessagesAsync(messageBatch);
				messagesSent += messageBatch.Count;
				messageBatch = await sender.CreateMessageBatchAsync();
			}
		}

		if (messageBatch.Count > 0)
		{
			await sender.SendMessagesAsync(messageBatch);
			messagesSent += messageBatch.Count;
		}

		return messagesSent;
	}
}
