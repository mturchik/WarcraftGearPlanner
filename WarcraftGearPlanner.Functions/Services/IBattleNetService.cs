using WarcraftGearPlanner.Functions.Models.Items;
using WarcraftGearPlanner.Functions.Models.Realms;

namespace WarcraftGearPlanner.Functions.Services;

public interface IBattleNetService
{
	Task<ItemClass?> GetItemClass(int itemClassId);
	Task<ItemClassIndex?> GetItemClassIndex();
	Task<RealmIndex?> GetRealmIndex();
}