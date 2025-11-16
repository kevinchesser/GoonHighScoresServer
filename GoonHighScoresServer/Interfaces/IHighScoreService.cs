using GoonHighScoresServer.Models;

namespace GoonHighScoresServer.Interfaces
{
    public interface IHighScoreService
    {
        Task<List<Character>> GetCharacters();
    }
}
