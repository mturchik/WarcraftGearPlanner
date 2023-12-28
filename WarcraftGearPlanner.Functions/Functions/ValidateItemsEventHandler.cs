using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using WarcraftGearPlanner.Functions.Extensions;
using WarcraftGearPlanner.Functions.Models.Enum;
using WarcraftGearPlanner.Functions.Models.Events;
using WarcraftGearPlanner.Functions.Models.Items;
using WarcraftGearPlanner.Functions.Models.Search;
using WarcraftGearPlanner.Functions.Services;
using InventoryTypeDTO = WarcraftGearPlanner.Shared.Models.Items.InventoryType;
using ItemDTO = WarcraftGearPlanner.Shared.Models.Items.Item;
using ItemQualityDTO = WarcraftGearPlanner.Shared.Models.Items.ItemQuality;

namespace WarcraftGearPlanner.Functions.Functions;

public class ValidateItemsEventHandler
{
	private readonly IBattleNetService _battleNetService;
	private readonly IApiService _apiService;
	private readonly ServiceBusSender _serviceBusSender;
	private ILogger _logger = null!;

	public ValidateItemsEventHandler(IBattleNetService battleNetService, IApiService apiService, ServiceBusClient _serviceBusClient)
	{
		_battleNetService = battleNetService;
		_apiService = apiService;
		_serviceBusSender = _serviceBusClient.CreateSender("validate-items-events");
	}

	[FunctionName("ValidateItemsEventHandler")]
	public async Task Run([ServiceBusTrigger("%SERVICE_BUS_QUEUE_VALIDATE_ITEMS%", Connection = "SERVICE_BUS_URL")] string myQueueItem, ILogger log)
	{
		_logger = log;
		_logger.LogInformation($"ValidateItemsEventHandler processed message: {myQueueItem}");

		var eventMessage = JsonConvert.DeserializeObject<ValidateItemsEvent>(myQueueItem ?? "")
			?? throw new ArgumentNullException(nameof(myQueueItem));

		_logger.LogInformation($"ValidateItemsEventHandler Processing: | {eventMessage.LogString}");

		await ProcessValidateItemsEvent(eventMessage);

		_logger.LogInformation($"ValidateItemsEventHandler Processed: | {eventMessage.LogString}");
	}

	private async Task ProcessValidateItemsEvent(ValidateItemsEvent eventMessage)
	{
		var itemRequest = new SearchRequest<ItemSearchParameters>
		{
			Page = 1,
			OrderBy = "id",
			OrderDirection = OrderDirection.Asc,
			Parameters = new()
			{
				MinId = eventMessage.MinId,
				MaxId = eventMessage.MaxId,
				ItemClassId = eventMessage.ClassId,
				ItemSubclassId = eventMessage.SubclassId,
				InventoryType = eventMessage.InventoryTypeType,
				Quality = eventMessage.ItemQualityType,
			}
		};

		var searchResponse = await _battleNetService.SearchItems(itemRequest);
		if (searchResponse is null || searchResponse.Results.Count == 0)
		{
			_logger.LogWarning($"No items found | {eventMessage.LogString}");
			return;
		}

		if (searchResponse.ResultCountCapped)
		{
			_logger.LogWarning($"Result count capped | {eventMessage.LogString}");

			await PublishValidateItemsEvents(eventMessage, itemRequest, searchResponse);

			return;
		}

		while (searchResponse.Page <= searchResponse.PageCount && searchResponse.Results.Count > 0)
		{
			_logger.LogInformation(
				$"Retrieved page [ {searchResponse.Page} / {searchResponse.PageCount} ] " +
				$"with {searchResponse.Results.Count} results."
			);

			await PutItemSearchResults(eventMessage, searchResponse);

			itemRequest.Page++;
			searchResponse = await _battleNetService.SearchItems(itemRequest);
			if (searchResponse is null) return;
		}
	}

	private async Task PutItemSearchResults(ValidateItemsEvent eventMessage, SearchResponse<ItemSearchResult> searchResponse)
	{
		var items = searchResponse.Results
			.Where(s => s.Data is not null)
			.Select(s => s.Data)
			.Select(d => new ItemDTO
			{
				ItemId = d!.Id,
				Level = d.Level,
				Name = d.Name?.American,
				RequiredLevel = d.RequiredLevel,
				MaxCount = d.MaxCount,
				PurchaseQuantity = d.PurchaseQuantity,
				PurchasePrice = d.PurchasePrice,
				SellPrice = d.SellPrice,
				IsEquippable = d.IsEquippable,
				IsStackable = d.IsStackable,
				ItemClassId = eventMessage.ItemClassId,
				ItemSubclassId = eventMessage.ItemSubclassId,
				ItemQualityId = eventMessage.ItemQualityId,
				ItemQualityType = d.Quality?.Type,
				ItemQualityName = d.Quality?.Name?.American,
				InventoryTypeId = eventMessage.InventoryTypeId,
				InventoryTypeType = d.InventoryType?.Type,
				InventoryTypeName = d.InventoryType?.Name?.American,
			})
			.ToList();
		_logger.LogInformation($"Merging {items.Count} items...");

		try
		{
			var putItems = await _apiService.Put("/items/search-results", items) ?? new();
			_logger.LogInformation($"Merged {putItems.Count} items!");
		}
		catch (ApplicationException ex)
		{
			_logger.LogError(ex, "Error calling API");
		}
	}

	private async Task PublishValidateItemsEvents(
		ValidateItemsEvent eventMessage,
		SearchRequest<ItemSearchParameters> itemRequest,
		SearchResponse<ItemSearchResult>? searchResponse)
	{
		if (eventMessage.ItemQualityType is null)
		{
			_logger.LogInformation("Publishing ValidateItemsEvents by ItemQuality");

			var itemQualities = await _apiService.Get<List<ItemQualityDTO>>("/item-qualities") ?? new();
			_logger.LogInformation($"Retrieved {itemQualities.Count} item qualities from API");

			var validateItemsEvents = itemQualities.Select(iq => new ValidateItemsEvent
			{
				ItemClassId = eventMessage.ItemClassId,
				ClassId = eventMessage.ClassId,
				ItemSubclassId = eventMessage.ItemSubclassId,
				SubclassId = eventMessage.SubclassId,
				InventoryTypeId = eventMessage.InventoryTypeId,
				InventoryTypeType = eventMessage.InventoryTypeType,
				ItemQualityId = iq.Id,
				ItemQualityType = iq.Type,
			}).ToList();
			_logger.LogInformation($"Created {validateItemsEvents.Count} Validate Items Events");

			var messagesSent = await _serviceBusSender.SendEventsAsync(validateItemsEvents);
			_logger.LogInformation($"Sent {messagesSent} messages");
		}
		else if (eventMessage.InventoryTypeType is null)
		{
			_logger.LogInformation("Publishing ValidateItemsEvents by InventoryType");

			var inventoryTypes = await _apiService.Get<List<InventoryTypeDTO>>("/inventory-types") ?? new();
			_logger.LogInformation($"Retrieved {inventoryTypes.Count} inventory types from API");

			var validateItemsEvents = inventoryTypes.Select(it => new ValidateItemsEvent
			{
				ItemClassId = eventMessage.ItemClassId,
				ClassId = eventMessage.ClassId,
				ItemSubclassId = eventMessage.ItemSubclassId,
				SubclassId = eventMessage.SubclassId,
				ItemQualityId = eventMessage.ItemQualityId,
				ItemQualityType = eventMessage.ItemQualityType,
				InventoryTypeId = it.Id,
				InventoryTypeType = it.Type,
			}).ToList();
			_logger.LogInformation($"Created {validateItemsEvents.Count} Validate Items Events");

			var messagesSent = await _serviceBusSender.SendEventsAsync(validateItemsEvents);
			_logger.LogInformation($"Sent {messagesSent} messages");
		}
		else if (eventMessage.MinId is null && eventMessage.MaxId is null)
		{
			_logger.LogInformation("Publishing ValidateItemsEvents by MinId");

			itemRequest.Page = 10;

			List<ValidateItemsEvent> validateItemsEvents = new();
			while (searchResponse?.ResultCountCapped ?? false)
			{
				searchResponse = await _battleNetService.SearchItems(itemRequest);
				var referenceData = searchResponse?.Results.LastOrDefault()?.Data;

				_logger.LogInformation(
					$"Retrieved page 10 with Id Range of [ {itemRequest.Parameters?.MinId} - {itemRequest.Parameters?.MaxId} ] " +
					$"with {searchResponse?.Results.Count} results."
				);

				var newEvent = new ValidateItemsEvent()
				{
					ItemClassId = eventMessage.ItemClassId,
					ClassId = eventMessage.ClassId,
					ItemSubclassId = eventMessage.ItemSubclassId,
					SubclassId = eventMessage.SubclassId,
					InventoryTypeId = eventMessage.InventoryTypeId,
					InventoryTypeType = eventMessage.InventoryTypeType,
					ItemQualityId = eventMessage.ItemQualityId,
					ItemQualityType = eventMessage.ItemQualityType,
					MinId = itemRequest.Parameters?.MinId,
					MaxId = (int?)referenceData?.Id,
				};
				validateItemsEvents.Add(newEvent);
				_logger.LogInformation($"Creating event: {newEvent.LogString}");

				if (itemRequest.Parameters != null)
					itemRequest.Parameters.MinId = (int?)referenceData?.Id;
			}
			_logger.LogInformation($"Created {validateItemsEvents.Count} Validate Items Events");

			var messagesSent = await _serviceBusSender.SendEventsAsync(validateItemsEvents);
			_logger.LogInformation($"Sent {messagesSent} messages");
		}
		else
			throw new ApplicationException("Too much data for all parameters to cover!");
	}
}
