using Leaderboard.Exceptions;
using Leaderboard.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Leaderboard.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private LeaderboardContext _context;

        public LeaderboardService()
        {
        }
        public LeaderboardService(LeaderboardContext context)
        {
            _context = context;
        }

        public void SetDBContext(LeaderboardContext context)
        {
            _context = context;
        }

        public List<LeaderboardEntry> GetFullLeaderboard(int limit = 10)
        {
            return _context.LeaderboardEntries
                .Include(x => x.Player)
                .OrderByDescending(x => x.TotalScore)
                .Take(limit)
                .ToList();
        }

        public LeaderboardEntry GetLeaderboardEntry(long id)
        {
            var leaderboardEntry = _context.LeaderboardEntries.Find(id);

            if (leaderboardEntry == null)
            {
                throw new LeaderboardServiceNotFoundException();
            }

            return leaderboardEntry;
        }

        public LeaderboardEntry CreateLeaderboardEntry(LeaderboardEntryDTO leaderboardEntryDTO)
        {
            var player = _context.Players.Find(leaderboardEntryDTO.PlayerId);

            if (player == null)
            {
                // Must have a valid player Id
                throw new LeaderboardServiceBadRequestException();
            }

            if (_context.LeaderboardEntries.Where(x => x.PlayerId == leaderboardEntryDTO.PlayerId).Count() != 0)
            {
                // Can only have 1 entry per player
                throw new LeaderboardServiceBadRequestException(); ;
            }

            var leaderboardEntry = new LeaderboardEntry
            {
                PlayerId = leaderboardEntryDTO.PlayerId,
                GamesPlayed = 0,
                TotalScore = 0
            };

            _context.LeaderboardEntries.Add(leaderboardEntry);
            _context.SaveChanges();

            return leaderboardEntry;
        }

        public void UpdateLeaderboardEntry(long id, LeaderboardEntryDTO leaderboardEntryDTO)
        {
            if (id != leaderboardEntryDTO.Id)
            {
                throw new LeaderboardServiceBadRequestException();
            }

            var leaderboardEntry = _context.LeaderboardEntries.Find(id);
            if (leaderboardEntry == null)
            {
                throw new LeaderboardServiceNotFoundException();
            }

            if (leaderboardEntry.PlayerId != leaderboardEntryDTO.PlayerId && (!PlayerExists(leaderboardEntryDTO.PlayerId) || LeaderboardEntryWithPlayerIdExist(leaderboardEntryDTO.PlayerId)))
            {
                throw new LeaderboardServiceBadRequestException();
            }

            leaderboardEntry.PlayerId = leaderboardEntryDTO.PlayerId;
            leaderboardEntry.GamesPlayed = leaderboardEntryDTO.GamesPlayed;
            leaderboardEntry.TotalScore = leaderboardEntryDTO.TotalScore;

            try
            {
                 _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaderboardEntryExists(id))
                {
                    throw new LeaderboardServiceNotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }
        public void PostNewScore(NewScoreDTO newScoreDTO)
        {
            var leaderboardEntry = _context.LeaderboardEntries.Where(x => x.PlayerId == newScoreDTO.PlayerId).FirstOrDefault();

            if (leaderboardEntry == null)
            {
                // No leaderboard entry for this player
                throw new LeaderboardServiceBadRequestException();
            }

            leaderboardEntry.GamesPlayed++;

            // Check if new high score
            if (newScoreDTO.TotalScore > leaderboardEntry.TotalScore)
            {
                leaderboardEntry.TotalScore = newScoreDTO.TotalScore;
            }

            _context.SaveChanges();
        }

        public void DeleteLeaderboardEntry(long id)
        {
            var leaderboardEntry = _context.LeaderboardEntries.Find(id);
            if (leaderboardEntry == null)
            {
                throw new LeaderboardServiceNotFoundException();
            }

            _context.LeaderboardEntries.Remove(leaderboardEntry);
            _context.SaveChanges();
        }

        private bool LeaderboardEntryExists(long id)
        {
            return _context.LeaderboardEntries.Any(e => e.Id == id);
        }

        private bool LeaderboardEntryWithPlayerIdExist(long playerId)
        {
            return _context.LeaderboardEntries.Any(e => e.PlayerId == playerId);
        }

        private bool PlayerExists(long id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
