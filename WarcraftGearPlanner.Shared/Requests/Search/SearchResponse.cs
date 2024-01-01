namespace WarcraftGearPlanner.Shared.Requests.Search;

public class SearchResponse<TResult>
{
	public int Page { get; set; }
	public int PageSize { get; set; }
	public int MaxPageSize { get; set; }
	public int PageCount { get; set; }
	public bool ResultCountCapped { get; set; }
	public List<TResult> Results { get; set; } = new();

	public SearchResponse() { }

	public static SearchResponse<TResult> FromRequestResults<TParameters>(
		SearchRequest<TParameters> searchRequest,
		List<TResult> results,
		int count
	) where TParameters : class, ISearchParameters
	=> new()
	{
		Page = searchRequest.GetPage(),
		PageSize = results.Count,
		MaxPageSize = searchRequest.GetPageSize(),
		PageCount = (int)Math.Ceiling((double)count / searchRequest.GetPageSize()),
		ResultCountCapped = searchRequest.GetPageSize() > searchRequest.PageSize,
		Results = results
	};
}
