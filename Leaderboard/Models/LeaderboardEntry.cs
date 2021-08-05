using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Models
{
    public class LeaderboardEntry
    {
        public long Id { get; set; }

        [Required]
        public long GamesPlayed { get; set; }

        [Required]
        public long TotalScore { get; set; }

        [Required]
        public long PlayerId { get; set; }

        virtual public Player Player { get; set; }
        public LeaderboardEntryDTO ToLeaderboardEntryDTO()
        {
            return new LeaderboardEntryDTO
            {
                Id = this.Id,
                PlayerId = this.PlayerId,
                GamesPlayed = this.GamesPlayed,
                TotalScore = this.TotalScore
            };
        }

        public LeaderboardEntryWithPlayerDTO ToLeaderboardWithPlayerDTO()
        {
            return new LeaderboardEntryWithPlayerDTO
            {
                Id = this.Id,
                PlayerId = this.PlayerId,
                FirstName = this.Player.FirstName,
                LastName = this.Player.LastName,
                GamesPlayed = this.GamesPlayed,
                TotalScore = this.TotalScore
            };
        }
    }
}