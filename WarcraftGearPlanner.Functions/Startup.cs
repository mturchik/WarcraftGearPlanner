using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using WarcraftGearPlanner.Functions.Services;

[assembly: FunctionsStartup(typeof(WarcraftGearPlanner.Functions.Startup))]
namespace WarcraftGearPlanner.Functions;
internal class Startup : FunctionsStartup
{
	public override void Configure(IFunctionsHostBuilder builder)
	{
		builder.Services.AddMemoryCache();
		builder.Services.AddAzureClients(builder =>
		{
			builder.AddServiceBusClient(Environment.GetEnvironmentVariable("SERVICE_BUS_URL"));
		});

		builder.Services.AddHttpClient("BattleNetService");
		builder.Services.AddHttpClient("ApiService");

		builder.Services.AddScoped<IBattleNetService, BattleNetService>();
		builder.Services.AddScoped<IApiService, ApiService>();
	}
}
