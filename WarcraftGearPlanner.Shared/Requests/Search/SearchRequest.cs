using WarcraftGearPlanner.Shared.Enum;

namespace WarcraftGearPlanner.Shared.Requests.Search;

public class SearchRequest<TParameters> where TParameters : class, ISearchParameters
{
	public int Page { get; set; }
	public int PageSize { get; set; }
	public string? OrderProperty { get; set; }
	public OrderDirection? OrderDirection { get; set; }
	public TParameters? Parameters { get; set; }

	public int GetPage() => Page <= 0 ? 1 : Page;
	public int GetPageSize() => PageSize <= 0 ? 10 : PageSize >= 200 ? 200 : PageSize;
}
