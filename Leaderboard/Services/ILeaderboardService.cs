using Leaderboard.Models;
using System.Collections.Generic;

namespace Leaderboard.Services
{
    public interface ILeaderboardService
    {
        void SetDBContext(LeaderboardContext context);
        List<LeaderboardEntry> GetFullLeaderboard(int limit);
        LeaderboardEntry GetLeaderboardEntry(long id);
        LeaderboardEntry CreateLeaderboardEntry(LeaderboardEntryDTO leaderboardEntryDTO);
        void UpdateLeaderboardEntry(long id, LeaderboardEntryDTO leaderboardEntryDTO);
        void PostNewScore(NewScoreDTO newScoreDTO);
        void DeleteLeaderboardEntry(long id);
    }
}
