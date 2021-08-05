namespace Leaderboard.Models
{
    public class LeaderboardEntryDTO
    {
        public long Id { get; set; }
        public long PlayerId { get; set; }
        public long GamesPlayed { get; set; }
        public long TotalScore { get; set; }
    }
}