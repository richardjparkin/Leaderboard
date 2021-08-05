using Leaderboard.Controllers;
using Leaderboard.Exceptions;
using Leaderboard.Models;
using Leaderboard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaderboardTests.Controllers
{
    [TestClass]
    public class PlayerControllerTests
    {
        private Mock<LeaderboardContext> mockLeaderboardContext;
        private Player player;
        private PlayerDTO playerDTO;
        public PlayerControllerTests()
        {
            mockLeaderboardContext = new Mock<LeaderboardContext>();
            player = new Player
            {
                Id = 1,
                FirstName = "Joe",
                LastName = "Bloggs",
                Email = "joe.bloggs@email.co.uk"
            };

            playerDTO = new PlayerDTO
            {
                Id = 1,
                FirstName = "Joe",
                LastName = "Bloggs",
                Email = "joe.bloggs@email.co.uk"
            };
        }

        [TestInitialize()]
        public void Startup()
        {
        }

        [TestCleanup()]
        public void Cleanup()
        {

        }

        [TestMethod]
        public async Task PlayersController_GetPlayers_All()
        {
            // Arrange
            var mockPlayerService = new Mock<IPlayersService>();
            mockPlayerService.Setup(x => x.GetAllPlayers()).Returns(new List<Player>());

            var playersController = new PlayersController(mockLeaderboardContext.Object, mockPlayerService.Object);

            // Act
            var result = await playersController.GetPlayers(null);

            // Assert
            mockPlayerService.VerifyAll();
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<PlayerDTO>));
        }

        [TestMethod]
        public async Task PlayersController_GetPlayers_Email()
        {
            // Arrange
            var mockPlayerService = new Mock<IPlayersService>();
            mockPlayerService.Setup(x => x.GetPlayerByEmail(It.IsAny<string>())).Returns(new List<Player>());

            var playersController = new PlayersController(mockLeaderboardContext.Object, mockPlayerService.Object);

            // Act
            var result = await playersController.GetPlayers("joe.bloggs@email.co.uk");

            // Assert
            mockPlayerService.VerifyAll();
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<PlayerDTO>));
        }

        [TestMethod]
        public async Task PlayersController_GetPlayer_Found()
        {
            // Arrange
            var mockPlayerService = new Mock<IPlayersService>();
            mockPlayerService.Setup(x => x.GetPlayerById(It.IsAny<long>())).Returns(player);

            var playersController = new PlayersController(mockLeaderboardContext.Object, mockPlayerService.Object);

            // Act
            var result = await playersController.GetPlayer(1);

            // Assert
            mockPlayerService.VerifyAll();
            Assert.IsInstanceOfType(result.Value, typeof(PlayerDTO));
        }

        [TestMethod]
        public async Task PlayersController_GetPlayer_NotFound()
        {
            // Arrange
            var mockPlayerService = new Mock<IPlayersService>();
            mockPlayerService.Setup(x => x.GetPlayerById(It.IsAny<long>())).Throws(new PlayersServiceNotFoundException());

            var playersController = new PlayersController(mockLeaderboardContext.Object, mockPlayerService.Object);

            // Act
            var result = await playersController.GetPlayer(1);
            
            // Assert
            mockPlayerService.VerifyAll();
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PlayersController_CreatedPlayer_Valid()
        {
            // Arrange
            var mockPlayerService = new Mock<IPlayersService>();
            mockPlayerService.Setup(x => x.CreatePlayer(It.IsAny<PlayerDTO>())).Returns(player);

            var playersController = new PlayersController(mockLeaderboardContext.Object, mockPlayerService.Object);

            // Act
            var result = await playersController.CreatePlayer(playerDTO);

            // Assert
            mockPlayerService.VerifyAll();
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public async Task PlayersController_CreatedPlayer_BadRequest()
        {
            // Arrange
            var mockPlayerService = new Mock<IPlayersService>();
            mockPlayerService.Setup(x => x.CreatePlayer(It.IsAny<PlayerDTO>())).Throws(new PlayersServiceBadRequestException());

            var playersController = new PlayersController(mockLeaderboardContext.Object, mockPlayerService.Object);

            // Act
            var result = await playersController.CreatePlayer(playerDTO);

            // Assert
            mockPlayerService.VerifyAll();
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task PlayersController_UpdatePlayer_NoContent()
        {
            // Arrange
            var mockPlayerService = new Mock<IPlayersService>();
            mockPlayerService.Setup(x => x.UpdatePlayer(It.IsAny<long>(), It.IsAny<PlayerDTO>()));

            var playersController = new PlayersController(mockLeaderboardContext.Object, mockPlayerService.Object);

            // Act
            var result = await playersController.UpdatePlayer(1, playerDTO);

            // Assert
            mockPlayerService.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task PlayersController_UpdatePlayer_BadRequest()
        {
            // Arrange
            var mockPlayerService = new Mock<IPlayersService>();
            mockPlayerService.Setup(x => x.UpdatePlayer(It.IsAny<long>(), It.IsAny<PlayerDTO>())).Throws(new PlayersServiceBadRequestException());

            var playersController = new PlayersController(mockLeaderboardContext.Object, mockPlayerService.Object);

            // Act
            var result = await playersController.UpdatePlayer(1, playerDTO);

            // Assert
            mockPlayerService.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task PlayersController_UpdatePlayer_NotFound()
        {
            // Arrange
            var mockPlayerService = new Mock<IPlayersService>();
            mockPlayerService.Setup(x => x.UpdatePlayer(It.IsAny<long>(), It.IsAny<PlayerDTO>())).Throws(new PlayersServiceNotFoundException());

            var playersController = new PlayersController(mockLeaderboardContext.Object, mockPlayerService.Object);

            // Act
            var result = await playersController.UpdatePlayer(1, playerDTO);

            // Assert
            mockPlayerService.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PlayersController_DeletePlayer_Valid()
        {
            // Arrange
            var mockPlayerService = new Mock<IPlayersService>();
            mockPlayerService.Setup(x => x.DeletePlayer(It.IsAny<long>()));

            var playersController = new PlayersController(mockLeaderboardContext.Object, mockPlayerService.Object);

            // Act
            var result = await playersController.DeletePlayer(1);

            // Assert
            mockPlayerService.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task PlayersController_DeletePlayer_NotFound()
        {
            // Arrange
            var mockPlayerService = new Mock<IPlayersService>();
            mockPlayerService.Setup(x => x.DeletePlayer(It.IsAny<long>())).Throws(new PlayersServiceNotFoundException());

            var playersController = new PlayersController(mockLeaderboardContext.Object, mockPlayerService.Object);

            // Act
            var result = await playersController.DeletePlayer(1);

            // Assert
            mockPlayerService.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


    }
}