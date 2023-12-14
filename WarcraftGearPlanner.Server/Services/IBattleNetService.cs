using WarcraftGearPlanner.Server.Models;
using WarcraftGearPlanner.Server.Models.Response;

namespace WarcraftGearPlanner.Server.Services;

public interface IBattleNetService
{
	Task<CharacterProfile?> GetCharacterProfile(string realmSlug, string characterName);
	Task<EquipmentSummary?> GetEquipmentSummary(string realmSlug, string characterName);
	Task<MediaReference?> GetItemMedia(int itemId);
	Task<IEnumerable<MediaReference>> GetItemMedia(int[] itemIds);
	Task<RealmIndexResponse?> GetRealmIndex();
}