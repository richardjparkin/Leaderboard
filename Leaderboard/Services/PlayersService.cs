using Leaderboard.Exceptions;
using Leaderboard.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Leaderboard.Services
{
    public class PlayersService : IPlayersService
    {
        private LeaderboardContext _context;

        public PlayersService()
        {
        }

        public PlayersService(LeaderboardContext context)
        {
            _context = context;
        }

        public void SetDBContext(LeaderboardContext context)
        {
            _context = context;
        }

        public List<Player> GetAllPlayers()
        {
            return _context.Players.ToList();
        }

        public List<Player> GetPlayerByEmail(string email)
        {
            return _context.Players.Where(x => x.Email == email).ToList();
        }

        public Player GetPlayerById(long id)
        {
            Player player =_context.Players.Find(id);

            if (player == null)
            {
                throw new PlayersServiceNotFoundException();
            }

            return player;
        }

        public Player CreatePlayer(PlayerDTO playerDTO)
        {
            if (!PlayerDTOValid(playerDTO) || EmailExists(playerDTO.Email))
            {
                throw new PlayersServiceBadRequestException();
            }

            var player = new Player
            {
                FirstName = playerDTO.FirstName,
                LastName = playerDTO.LastName,
                Email = playerDTO.Email
            };

            _context.Players.Add(player);
            _context.SaveChanges();

            return player;
        }

        public void UpdatePlayer(long id, PlayerDTO playerDTO)
        {
            if (id != playerDTO.Id)
            {
                throw new PlayersServiceBadRequestException();
            }

            if (!PlayerDTOValid(playerDTO))
            {
                throw new PlayersServiceBadRequestException();
            }

            var player = _context.Players.Find(id);
            if (player == null)
            {
                throw new PlayersServiceNotFoundException();
            }

            if (player.Email != playerDTO.Email && EmailExists(playerDTO.Email))
            {
                throw new PlayersServiceBadRequestException();
            }

            player.FirstName = playerDTO.FirstName;
            player.LastName = playerDTO.LastName;
            player.Email = playerDTO.Email;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) when (!PlayerExists(id))
            {
                throw new PlayersServiceNotFoundException(); 
            }
        }

        public void DeletePlayer(long id)
        {
            var player = _context.Players.Find(id);
            if (player == null)
            {
                throw new PlayersServiceNotFoundException();
            }

            _context.Players.Remove(player);
            _context.SaveChangesAsync();
        }

        public bool PlayerDTOValid(PlayerDTO playerDTO)
        {
            return playerDTO.FirstName.Length > 0 &&
                playerDTO.LastName.Length > 0 &&
                EmailValid(playerDTO.Email);
        }

        private bool PlayerExists(long id)
        {
            return _context.Players.Any(e => e.Id == id);
        }

        private bool EmailExists(string email)
        {
            return _context.Players.Any(e => e.Email == email);
        }

        private bool EmailValid(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
