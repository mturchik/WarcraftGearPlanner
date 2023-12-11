using WarcraftGearPlanner.Models;
using WarcraftGearPlanner.Models.Response;

namespace WarcraftGearPlanner.Services;

public interface IBattleNetService
{
	Task<CharacterProfile?> GetCharacterProfile(string name, string realm);
	Task<EquipmentSummary?> GetEquipmentSummary(string name, string realm);
	Task<MediaReference?> GetItemMedia(int id);
	Task<RealmIndexResponse?> GetRealms();
}