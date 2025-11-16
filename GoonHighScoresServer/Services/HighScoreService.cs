using GoonHighScoresServer.Interfaces;
using GoonHighScoresServer.Models;

namespace GoonHighScoresServer.Services
{
    public class HighScoreService : IHighScoreService
    {
        private readonly IHighScoreRepository _highScoreRepository;

        public HighScoreService(IHighScoreRepository highScoreRepository)
        {
            _highScoreRepository = highScoreRepository;
        }

        public async Task<List<Character>> GetCharacters()
        {
            return await _highScoreRepository.GetCharacters();
        }

        public async Task RecordXpDropsIfNecessary(Character character, OsrsCharacterStats osrsCharacterStats, string processingTime)
        {
            List<XpDrop> xpDropsToRecord = new List<XpDrop>(24);
            int? mostRecentOverallXp = await _highScoreRepository.GetMostRecentOverallXp(character.Id);
            if(!mostRecentOverallXp.HasValue)
            {
                foreach(OsrsSkill osrsSkill in osrsCharacterStats.Skills)
                {
                    XpDrop xpDrop = new XpDrop()
                    {
                        CharacterId = character.Id,
                        SkillId = osrsSkill.Id,
                        Xp = osrsSkill.Xp,
                        Level = osrsSkill.Level,
                        Rank = osrsSkill.Rank
                    };
                    xpDropsToRecord.Add(xpDrop);
                }
                await _highScoreRepository.SaveXpDrops(xpDropsToRecord, processingTime);
                return;
            }

        }
    }
}
