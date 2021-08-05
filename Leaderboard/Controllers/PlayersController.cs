using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Leaderboard.Models;
using Leaderboard.Exceptions;
using Leaderboard.Services;

namespace Leaderboard.Controllers
{
    [Route("api/Players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly LeaderboardContext _context;

        private IPlayersService playersService;

        public PlayersController(LeaderboardContext context, [FromServices] IPlayersService playersService)
        {
            _context = context;
            this.playersService = playersService;
            this.playersService.SetDBContext(_context);
        }

        // GET: api/Players - Returns all players or returns player with given email address
        // IMPROVEMENTS: Returning all players - A few test records is fine but going forward it would be better to implement a paging mechanism e.g. in blocks of 100 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayers(string email)
        {
            if (email != null)
            {
                return playersService.GetPlayerByEmail(email)
                    .Select(x => x.ToPlayerDTO())
                    .ToList();
            }
            else
            {
                return playersService.GetAllPlayers()
                    .Select(x => x.ToPlayerDTO())
                    .ToList();
            }
        }

        // GET: api/Players/{id} - Gets the player with a given Id
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDTO>> GetPlayer(long id)
        {
            try
            {
                return playersService.GetPlayerById(id).ToPlayerDTO();
            }
            catch (PlayersServiceNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Players - Creates a new player
        [HttpPost]
        public async Task<ActionResult<PlayerDTO>> CreatePlayer(PlayerDTO playerDTO)
        {
            try
            {
                Player player = playersService.CreatePlayer(playerDTO);

                return CreatedAtAction(
                    nameof(GetPlayer),
                    new { id = player.Id },
                    player.ToPlayerDTO());

            }
            catch (PlayersServiceBadRequestException)
            {
                return BadRequest();
            }
        }

        // PUT: api/Players/{id} - Updates the player with a given Id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(long id, PlayerDTO playerDTO)
        {
            try
            {
                playersService.UpdatePlayer(id, playerDTO);
                return NoContent();
            }
            catch (PlayersServiceBadRequestException)
            {
                return BadRequest();
            }
            catch (PlayersServiceNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Players/{id} - Deletes the player with a given Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(long id)
        {
            try
            {
                playersService.DeletePlayer(id);
                return NoContent();
            }
             catch (PlayersServiceNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
