using Leaderboard.Models;
using System.Collections.Generic;


namespace Leaderboard.Services
{
    public interface IPlayersService
    {
        void SetDBContext(LeaderboardContext context);
        List<Player> GetAllPlayers();
        List<Player> GetPlayerByEmail(string email);
        Player GetPlayerById(long id);
        Player CreatePlayer(PlayerDTO playerDTO);
        void UpdatePlayer(long id, PlayerDTO playerDTO);
        void DeletePlayer(long id);
    }
}
