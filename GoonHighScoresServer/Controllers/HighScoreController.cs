using GoonHighScoresServer.Interfaces;
using GoonHighScoresServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoonHighScoresServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HighScoreController : ControllerBase
    {
        private readonly IHighScoreService _highScoreService;

        public HighScoreController(IHighScoreService highScoreService)
        {
            _highScoreService = highScoreService;
        }

        [HttpGet("characterNames")]
        public async Task<IActionResult> CharacterNames()
        {
            List<Character> characterNames = await _highScoreService.GetCharacters();
            return Ok(characterNames);
        }

        [HttpGet("{characterName}")]
        public async Task<IActionResult> GetCharacterOverview([FromRoute]string characterName)
        {
            return Ok();
        }


        [HttpGet("last24HourLeaderboard")]
        public async Task<IActionResult> Last24HourLeaderboard()
        {
            await _highScoreService.GetLastXTimeSpanOverallXpLeadboard(TimeSpan.FromHours(24));
            return Ok();
        }
    }
}
