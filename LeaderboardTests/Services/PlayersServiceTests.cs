using Leaderboard.Exceptions;
using Leaderboard.Models;
using Leaderboard.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeaderboardTests.Services
{
    [TestClass]
    public class PlayersServiceTests
    {
        private PlayersService playersService;
        private LeaderboardContext leaderboardContext;

        private static string firstName = "Joe";
        private static string lastName = "Bloggs";
        private static string email = "joe.bloggs@email.co.uk";

        [TestInitialize()]
        public void Startup()
        {
            // Simulate DbContext using in memory database
            // Test data is same as LeaderboardContext.OnModelCreating, although should probably be moved to here

            var options = new DbContextOptionsBuilder<LeaderboardContext>()
                .UseInMemoryDatabase("LeaderboardTest")
                .Options;

            leaderboardContext = new LeaderboardContext(options);
            playersService = new PlayersService(leaderboardContext);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            playersService = null;
            leaderboardContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void PlayersService_GetAllPlayers()
        {
            // Arrange
 
            // Act
            var players = playersService.GetAllPlayers();

            // Assert
            Assert.AreEqual(11, players.Count, "Incorrect number of players returned");
        }

        [TestMethod]
        public void PlayersService_GetAllPlayerByEmailPlayers_Valid()
        {
            // Arrange
            string email = "richard.parkin@email.co.uk";

            // Act
            var players = playersService.GetPlayerByEmail(email);

            // Assert
            Assert.AreEqual(1, players.Count, "Incorrect number of players returned");
            Assert.AreEqual(email, players[0].Email, "Email addresses don't match");
        }

        [TestMethod]
        public void PlayersService_GetAllPlayerByEmailPlayers_NotFound()
        {
            // Arrange
            string email = "richard.parkin@email1.co.uk";

            // Act
            var players = playersService.GetPlayerByEmail(email);

            // Assert
            Assert.AreEqual(0, players.Count, "Incorrect number of players returned");
        }

        [TestMethod]
        public void PlayersService_GetPlayerById_Valid()
        {
            // Arrange
            long id = 1;

            // Act
            var player = playersService.GetPlayerById(id);

            // Assert
            Assert.AreEqual(1, player.Id, "Id does not equal {0}", id);
        }

        [TestMethod]
        public void PlayersService_GetPlayerById_NotFound()
        {
            // Arrange
            long id = 100;

            // Act

            // Assert
            Assert.ThrowsException<PlayersServiceNotFoundException>(() => playersService.GetPlayerById(id));
        }

        [TestMethod]
        public void PlayersService_CreatePlayer_Valid()
        {
            // Arrange
            PlayerDTO playerDTO = new PlayerDTO
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            // Act
            var player = playersService.CreatePlayer(playerDTO);

            // Assert
            Assert.AreEqual(12, player.Id, "Id does not equal 1");
        }

        [TestMethod]
        public void PlayersService_CreatePlayer_PDOTnvalid()
        {
            // Arrange
            PlayerDTO playerDTO = new PlayerDTO
            {
                FirstName = "",
                LastName = lastName,
                Email = email
            };

            // Act

            // Assert
            Assert.ThrowsException<PlayersServiceBadRequestException>(() => playersService.CreatePlayer(playerDTO));
        }

        [TestMethod]
        public void PlayersService_UpdatePlayer_Valid()
        {
            // Arrange
            PlayerDTO playerDTO = new PlayerDTO
            {
                Id = 1,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            // Act
            playersService.UpdatePlayer(1, playerDTO);

            // Assert
            // Framework will catch any thrown exceptions
        }

        [TestMethod]
        public void PlayersService_UpdatePlayer_IdsDontMatch()
        {
            // Arrange
            PlayerDTO playerDTO = new PlayerDTO
            {
                Id = 1,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            // Act
           
            // Assert
            Assert.ThrowsException<PlayersServiceBadRequestException>(() => playersService.UpdatePlayer(2, playerDTO));
        }

        [TestMethod]
        public void PlayersService_UpdatePlayer_PlayerDTOInvalid()
        {
            // Arrange
            PlayerDTO playerDTO = new PlayerDTO
            {
                Id = 1,
                FirstName = "",
                LastName = lastName,
                Email = email
            };

            // Act

            // Assert
            Assert.ThrowsException<PlayersServiceBadRequestException>(() => playersService.UpdatePlayer(1, playerDTO));
        }

        [TestMethod]
        public void PlayersService_UpdatePlayer_PlayerNotFound()
        {
            PlayerDTO playerDTO = new PlayerDTO
            {
                Id = 100,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            // Act

            // Assert
            Assert.ThrowsException<PlayersServiceNotFoundException>(() => playersService.UpdatePlayer(100, playerDTO));
        }

        [TestMethod]
        public void PlayersService_UpdatePlayer_EmailExists()
        {
            PlayerDTO playerDTO = new PlayerDTO
            {
                Id = 1,
                FirstName = firstName,
                LastName = lastName,
                Email = "pete.castle@email.co.uk"
            };

            // Act

            // Assert
            Assert.ThrowsException<PlayersServiceBadRequestException>(() => playersService.UpdatePlayer(1, playerDTO));
        }

        [TestMethod]
        public void PlayersService_DeletePlayer_Valid()
        {
            // Arrange
            long id = 1;

            // Act
            playersService.DeletePlayer(id);

            // Assert
            // Framework will catch any thrown exceptions
        }

        [TestMethod]
        public void PlayersService_DeletePlayer_NotFound()
        {
            // Arrange
            long id = 100;

            // Act

            // Assert
            Assert.ThrowsException<PlayersServiceNotFoundException>(() => playersService.DeletePlayer(id));
        }


        [TestMethod]
        public void PlayersService_PlayerDTOValid_Valid()
        {
            // Arrange
            PlayerDTO playerDTO = new PlayerDTO
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            // Act
            bool valid = playersService.PlayerDTOValid(playerDTO);

            // Assert
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void PlayersService_PlayerDTOValid_FirstNameInvalid()
        {
            // Arrange
            PlayerDTO playerDTO = new PlayerDTO
            {
                FirstName = "",
                LastName = lastName,
                Email = email
            };

            // Act
            bool valid = playersService.PlayerDTOValid(playerDTO);

            // Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void PlayersService_PlayerDTOValid_LastNameInvalid()
        {
            // Arrange
            PlayerDTO playerDTO = new PlayerDTO
            {
                FirstName = firstName,
                LastName = "",
                Email = email
            };

            // Act
            bool valid = playersService.PlayerDTOValid(playerDTO);

            // Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void PlayersService_PlayerDTOValid_EmailInvalid()
        {
            // Arrange
            PlayerDTO playerDTO = new PlayerDTO
            {
                FirstName = firstName,
                LastName = lastName,
                Email = "richardparkin.co.uk"
            };

            // Act
            bool valid = playersService.PlayerDTOValid(playerDTO);

            // Assert
            Assert.IsFalse(valid);
        }
    }
}

