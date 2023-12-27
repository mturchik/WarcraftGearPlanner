using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using WarcraftGearPlanner.Functions.Models.Enum;
using WarcraftGearPlanner.Functions.Models.Events;
using WarcraftGearPlanner.Functions.Models.Items;
using WarcraftGearPlanner.Functions.Models.Search;
using WarcraftGearPlanner.Functions.Services;
using ItemDTO = WarcraftGearPlanner.Shared.Models.Items.Item;

namespace WarcraftGearPlanner.Functions.Functions;

public class ValidateItemsEventHandler
{
	private readonly IBattleNetService _battleNetService;
	private readonly IApiService _apiService;
	private readonly ServiceBusSender _serviceBusSender;

	public ValidateItemsEventHandler(IBattleNetService battleNetService, IApiService apiService, ServiceBusClient _serviceBusClient)
	{
		_battleNetService = battleNetService;
		_apiService = apiService;
		_serviceBusSender = _serviceBusClient.CreateSender("validate-items-results-capped");
	}

	[FunctionName("ValidateItemsEventHandler")]
	public async Task Run([ServiceBusTrigger("%SERVICE_BUS_QUEUE_VALIDATE_ITEMS%", Connection = "SERVICE_BUS_URL")] string myQueueItem, ILogger log)
	{
		log.LogInformation($"ValidateItemsEventHandler processed message: {myQueueItem}");

		var eventMessage = JsonConvert.DeserializeObject<ValidateItemsEvent>(myQueueItem ?? "") ?? throw new ArgumentNullException(nameof(myQueueItem));
		log.LogInformation($"Processing Validate Items Event for Item Class {eventMessage.ClassId} and Item Subclass {eventMessage.SubclassId}");

		var itemRequest = new SearchRequest<ItemSearchParameters>
		{
			Page = 1,
			OrderBy = "id",
			OrderDirection = OrderDirection.Asc,
			Parameters = new()
			{
				ItemClassId = eventMessage.ClassId,
				ItemSubclassId = eventMessage.SubclassId,
			}
		};
		SearchResponse<ItemSearchResult>? searchResponse;
		do
		{
			searchResponse = await _battleNetService.SearchItems(itemRequest);
			log.LogInformation($"Retrieved page {searchResponse?.Page} of {searchResponse?.PageCount} with {searchResponse?.Results?.Count} items from Battle.net API");
			if (searchResponse is null) return;

			if (searchResponse.ResultCountCapped && searchResponse.Page == 1)
			{
				log.LogWarning("Result count capped, not all items were returned");
				await _serviceBusSender.SendMessageAsync(new ServiceBusMessage(JsonConvert.SerializeObject(eventMessage)));
			}

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
					ItemQualityType = d.Quality?.Type,
					ItemQualityName = d.Quality?.Name?.American,
					InventoryTypeType = d.InventoryType?.Type,
					InventoryTypeName = d.InventoryType?.Name?.American,
				})
				.ToList();
			log.LogInformation($"Sending {items.Count} items to API");

			try
			{
				var putItems = await _apiService.Put("/items/search-results", items) ?? new();
				log.LogInformation($"Created {putItems.Count} items in API");
			}
			catch (ApplicationException ex)
			{
				log.LogError(ex, "Error calling API");
			}

			itemRequest.Page++;
		} while (searchResponse.Page < searchResponse.PageCount);

		log.LogInformation($"Finished processing Validate Items Event for Item Class {eventMessage.ClassId} and Item Subclass {eventMessage.SubclassId}");
	}
}
