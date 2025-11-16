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
    }
}
