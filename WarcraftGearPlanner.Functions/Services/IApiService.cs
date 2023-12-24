
namespace WarcraftGearPlanner.Functions.Services;
public interface IApiService
{
	Task Delete(string url);
	Task<T?> Get<T>(string url);
	Task<T?> Post<T>(string url, T? body);
	Task<T?> Put<T>(string url, T? body);
}