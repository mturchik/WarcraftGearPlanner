using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using WarcraftGearPlanner.Functions.Services;

[assembly: FunctionsStartup(typeof(WarcraftGearPlanner.Functions.Startup))]
namespace WarcraftGearPlanner.Functions;
internal class Startup : FunctionsStartup
{
	public override void Configure(IFunctionsHostBuilder builder)
	{
		builder.Services.AddHttpClient();

		builder.Services.AddScoped<IBattleNetService, BattleNetService>();
	}
}
