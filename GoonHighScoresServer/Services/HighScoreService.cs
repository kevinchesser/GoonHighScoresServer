using GoonHighScoresServer.Interfaces;
using GoonHighScoresServer.Models;

namespace GoonHighScoresServer.Services
{
    public class HighScoreService : IHighScoreService
    {
        private readonly ILogger<HighScoreService> _logger;
        private readonly IHighScoreRepository _highScoreRepository;

        public HighScoreService(ILogger<HighScoreService> logger, IHighScoreRepository highScoreRepository)
        {
            _logger = logger;
            _highScoreRepository = highScoreRepository;
        }

        public async Task<List<Character>> GetCharacters()
        {
            return await _highScoreRepository.GetCharacters();
        }

        public Task<TimespanXpLeaderboardViewModel> GetLastXTimeSpanOverallXpLeadboard(TimeSpan lookbackTimeSpan)
        {
            throw new NotImplementedException();
        }

        public async Task RecordXpDropsIfNecessary(Character character, Dictionary<int, OsrsSkill> osrsCharacterStats, string processingTime)
        {
            List<XpDrop> xpDropsToRecord = new List<XpDrop>(24);
            int? mostRecentOverallXp = await _highScoreRepository.GetMostRecentOverallXp(character.Id);
            if(!mostRecentOverallXp.HasValue)
            {
                foreach(OsrsSkill osrsSkill in osrsCharacterStats.Values)
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
            }
            else
            {
                Dictionary<int, int> mostRecentXpDrops = await _highScoreRepository.GetMostRecentXpDrops(character.Id);
                foreach(OsrsSkill osrsSkill in osrsCharacterStats.Values)
                {
                    if (!mostRecentXpDrops.ContainsKey(osrsSkill.Id) || (mostRecentXpDrops.ContainsKey(osrsSkill.Id) && mostRecentXpDrops[osrsSkill.Id] < osrsSkill.Xp))
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
                }
            }

            if (xpDropsToRecord.Count > 0)
            {
                _logger.LogInformation("Record {numberOf} XpDrops for {CharacterId}", xpDropsToRecord.Count, character.Id);
                await _highScoreRepository.SaveXpDrops(xpDropsToRecord, processingTime);
            }
        }
    }
}
