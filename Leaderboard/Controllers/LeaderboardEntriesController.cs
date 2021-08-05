using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Leaderboard.Models;
using Leaderboard.Services;
using Leaderboard.Exceptions;

namespace Leaderboard.Controllers
{
    [Route("api/Leaderboard")]
    [ApiController]
    public class LeaderboardEntriesController : ControllerBase
    {
        private readonly LeaderboardContext _context;
        private ILeaderboardService leaderboardService;

        public LeaderboardEntriesController(LeaderboardContext context, [FromServices] ILeaderboardService leaderboardService)
        {
            _context = context;
            this.leaderboardService = leaderboardService;
            leaderboardService.SetDBContext(context);
        }

        // GET: api/Leaderboard - Gets leaderboard incuding player details, sorted by score, limited by query parameter
        // IMPROVEMENTS - Be nice to add a paging mechanism e.g. in blocks of 100
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaderboardEntryWithPlayerDTO>>> GetFullLeaderboard(int limit = 10)
        {
            return leaderboardService.GetFullLeaderboard(limit)
                .Select(x => x.ToLeaderboardWithPlayerDTO())
                .ToList();
        }

        // GET: api/Leaderboard/{i} - Gets a leaderboard entry by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaderboardEntryDTO>> GetLeaderboardEntry(long id)
        {
            try
            {
                return leaderboardService.GetLeaderboardEntry(id).ToLeaderboardEntryDTO();
            }
            catch (LeaderboardServiceNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/LeaderboardEntries - Creates a new leaderboard entry for a given player
        [HttpPost]
        public async Task<ActionResult<LeaderboardEntryDTO>> CreateLeaderboardEntry(LeaderboardEntryDTO leaderboardEntryDTO)
        {
            try
            {
                LeaderboardEntry leaderboardEntry = leaderboardService.CreateLeaderboardEntry(leaderboardEntryDTO);

                return CreatedAtAction(
                    nameof(GetLeaderboardEntry),
                    new { id = leaderboardEntry.Id },
                    leaderboardEntry.ToLeaderboardEntryDTO());
            }
            catch (LeaderboardServiceBadRequestException)
            {
                return BadRequest();
            }
        }

        // PUT: api/Leaderboard/{id} - Update a leaderboard entry with a given Id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeaderboardEntry(long id, LeaderboardEntryDTO leaderboardEntryDTO)
        {
            try
            {
                leaderboardService.UpdateLeaderboardEntry(id, leaderboardEntryDTO);
                return NoContent();
            }
            catch (LeaderboardServiceBadRequestException)
            {
                return BadRequest();
            }
            catch (LeaderboardServiceNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/LeaderboardEntries/scores
        [HttpPost("scores")]
        public async Task<ActionResult> PostNewScore(NewScoreDTO newScoreDTO)
        {
            try
            {
                leaderboardService.PostNewScore(newScoreDTO);
                return NoContent();
            }
            catch (LeaderboardServiceBadRequestException)
            {
                return BadRequest();
            } 
        }

        // DELETE: api/Leaderboard/{i} - Deletes a leaderboard entry with a give Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaderboardEntry(long id)
        {
            try
            {
                leaderboardService.DeleteLeaderboardEntry(id);
                return NoContent();
            }
            catch (LeaderboardServiceNotFoundException)
            {
                return NotFound();
            }            
        }
    }
}
