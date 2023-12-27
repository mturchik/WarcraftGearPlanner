using WarcraftGearPlanner.Functions.Models.Shared;

namespace WarcraftGearPlanner.Functions.Models.Search;

public class SearchResponse<T>
{
	public int Page { get; set; }
	public int PageSize { get; set; }
	public int MaxPageSize { get; set; }
	public int PageCount { get; set; }
	public bool ResultCountCapped { get; set; }
	public List<DataReference<T>> Results { get; set; } = new();
}
