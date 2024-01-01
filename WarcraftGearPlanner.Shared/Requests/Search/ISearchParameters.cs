namespace WarcraftGearPlanner.Shared.Requests.Search;

public interface ISearchParameters
{
	List<string> ToQueryParameters();
}
