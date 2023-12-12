using WarcraftGearPlanner.Models;
using WarcraftGearPlanner.Models.Response;

namespace WarcraftGearPlanner.Services;

public interface IBattleNetService
{
	Task<CharacterProfile?> GetCharacterProfile(string realmSlug, string characterName);
	Task<EquipmentSummary?> GetEquipmentSummary(string realmSlug, string characterName);
	Task<MediaReference?> GetItemMedia(int id);
	Task<IEnumerable<MediaReference>> GetItemMedia(int[] ids);
	Task<RealmIndexResponse?> GetRealms();
}