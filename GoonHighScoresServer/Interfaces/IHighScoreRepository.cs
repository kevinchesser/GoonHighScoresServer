using GoonHighScoresServer.Models;

namespace GoonHighScoresServer.Interfaces
{
    public interface IHighScoreRepository
    {
        Task<List<Character>> GetCharacters();
    }
}
