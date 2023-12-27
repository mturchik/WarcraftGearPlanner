using WarcraftGearPlanner.Functions.Models.Enum;

namespace WarcraftGearPlanner.Functions.Models.Search;

public class SearchRequest<T> where T : BaseSearchParameters
{
	public int Page { get; set; }
	public int PageSize { get; set; }
	public string? OrderBy { get; set; }
	public OrderDirection? OrderDirection { get; set; }
	public T? Parameters { get; set; }

	public string GetQueryString()
	{
		var criterium = new List<string>();

		if (Parameters is not null)
			criterium.AddRange(Parameters.ToCriterium());

		if (Page > 0)
			criterium.Add($"_page={Page}");

		if (PageSize > 0)
			criterium.Add($"_pageSize={PageSize}");

		if (!string.IsNullOrWhiteSpace(OrderBy))
		{
			var criteria = "orderby=";
			if (OrderDirection.HasValue)
				criteria += $"{OrderBy}:{OrderDirection.Value.ToString().ToLower()}";
			else
				criteria += OrderBy;
			criterium.Add(criteria);
		}

		return "?" + string.Join("&", criterium);
	}
}
