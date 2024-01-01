using WarcraftGearPlanner.Shared.Requests.Search;

namespace WarcraftGearPlanner.Functions.Models.Search;
public class BnetSearchRequest : SearchRequest<ItemSearchParameters>
{
	public string GetQueryString()
	{
		var criterium = new List<string>();

		if (Parameters is not null)
			criterium.AddRange(Parameters.ToQueryParameters());

		if (Page > 0)
			criterium.Add($"_page={Page}");

		if (PageSize > 0)
			criterium.Add($"_pageSize={PageSize}");

		if (!string.IsNullOrWhiteSpace(OrderProperty))
		{
			var criteria = "orderby=";
			if (OrderDirection.HasValue)
				criteria += $"{OrderProperty}:{OrderDirection.Value.ToString().ToLower()}";
			else
				criteria += OrderProperty;
			criterium.Add(criteria);
		}

		return "?" + string.Join("&", criterium);
	}
}
