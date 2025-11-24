namespace GoonHighScoresServer.Models
{
    public class TimespanXpLeaderboardViewModel
    {
        public required List<TimeSpanLeaderboardItem> TimeSpanLeaderboardItems { get; set; }
        public TimeSpan TimeSpanUsed { get; set; }
    }
}
