using GoonHighScoresServer.Models;

namespace GoonHighScoresServer.Interfaces
{
    public interface IHighScoreRepository
    {
        Task<List<Character>> GetCharacters();
        Task<List<XpDrop>> GetMostRecentXpDrops(int characterId);
        Task<int?> GetMostRecentOverallXp(int characterId);
        Task SaveXpDrops(List<XpDrop> xpDrops, string processingTime);
    }
}
