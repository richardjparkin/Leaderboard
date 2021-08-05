using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leaderboard.Models
{
    public class LeaderboardEntryWithPlayerDTO
    {
        public long Id { get; set; }
        public long PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long GamesPlayed { get; set; }
        public long TotalScore { get; set; }
    }
}
