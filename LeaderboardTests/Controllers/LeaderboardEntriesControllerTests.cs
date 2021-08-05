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
    public class LeaderboardEntriesControllerTests
    {
        private Mock<LeaderboardContext> mockLeaderboardContext;
        private LeaderboardEntry leaderboardEntry;
        private LeaderboardEntryDTO leaderboardEntryDTO;
        private NewScoreDTO newScoreDTO;
        public LeaderboardEntriesControllerTests()
        {
            mockLeaderboardContext = new Mock<LeaderboardContext>();
            leaderboardEntry = new LeaderboardEntry
            {
                Id = 1,
                PlayerId = 1,
                GamesPlayed = 1,
                TotalScore = 1
            };
            leaderboardEntryDTO = new LeaderboardEntryDTO
            {
                Id = 1,
                PlayerId = 1,
                GamesPlayed = 1,
                TotalScore = 1
            };
            newScoreDTO = new NewScoreDTO
            {
                PlayerId = 1,
                TotalScore = 1
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
        public async Task LeaderboardController_GetPlayers_GetFullLeaderboard()
        {
            // Arrange
            var mockLeaderboardService = new Mock<ILeaderboardService>();
            mockLeaderboardService.Setup(x => x.GetFullLeaderboard(It.IsAny<int>())).Returns(new List<LeaderboardEntry>());

            var leaderboardEntriesController = new LeaderboardEntriesController(mockLeaderboardContext.Object, mockLeaderboardService.Object);

            // Act
            var result = await leaderboardEntriesController.GetFullLeaderboard(100);

            // Assert
            mockLeaderboardService.VerifyAll();
            Assert.IsInstanceOfType(result.Value, typeof(IEnumerable<LeaderboardEntryWithPlayerDTO>));
        }

        [TestMethod]
        public async Task LeaderboardController_GetLeaderboardEntry_Found()
        {
            // Arrange
            var mockLeaderboardService = new Mock<ILeaderboardService>();
            mockLeaderboardService.Setup(x => x.GetLeaderboardEntry(It.IsAny<long>())).Returns(leaderboardEntry);

            var leaderboardEntriesController = new LeaderboardEntriesController(mockLeaderboardContext.Object, mockLeaderboardService.Object);

            // Act
            var result = await leaderboardEntriesController.GetLeaderboardEntry(1);

            // Assert
            mockLeaderboardService.VerifyAll();
            Assert.IsInstanceOfType(result.Value, typeof(LeaderboardEntryDTO));
        }

        [TestMethod]
        public async Task LeaderboardController_GetLeaderboardEntry_NotFound()
        {
            // Arrange
            var mockLeaderboardService = new Mock<ILeaderboardService>();
            mockLeaderboardService.Setup(x => x.GetLeaderboardEntry(It.IsAny<long>())).Throws(new LeaderboardServiceNotFoundException());

            var leaderboardEntriesController = new LeaderboardEntriesController(mockLeaderboardContext.Object, mockLeaderboardService.Object);

            // Act
            var result = await leaderboardEntriesController.GetLeaderboardEntry(1);

            // Assert
            mockLeaderboardService.VerifyAll();
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task LeaderboardController_CreateLeaderboardEntry_Valid()
        {
            // Arrange
            var mockLeaderboardService = new Mock<ILeaderboardService>();
            mockLeaderboardService.Setup(x => x.CreateLeaderboardEntry(It.IsAny<LeaderboardEntryDTO>())).Returns(leaderboardEntry);

            var leaderboardEntriesController = new LeaderboardEntriesController(mockLeaderboardContext.Object, mockLeaderboardService.Object);

            // Act
            var result = await leaderboardEntriesController.CreateLeaderboardEntry(leaderboardEntryDTO);

            // Assert
            mockLeaderboardService.VerifyAll();
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public async Task LeaderboardController_CreateLeaderboardEntry_BadRequest()
        {
            // Arrange
            var mockLeaderboardService = new Mock<ILeaderboardService>();
            mockLeaderboardService.Setup(x => x.CreateLeaderboardEntry(It.IsAny<LeaderboardEntryDTO>())).Throws(new LeaderboardServiceBadRequestException());

            var leaderboardEntriesController = new LeaderboardEntriesController(mockLeaderboardContext.Object, mockLeaderboardService.Object);

            // Act
            var result = await leaderboardEntriesController.CreateLeaderboardEntry(leaderboardEntryDTO);

            // Assert
            mockLeaderboardService.VerifyAll();
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task LeaderboardController_UpdateLeaderboardEntry_Valid()
        {
            // Arrange
            var mockLeaderboardService = new Mock<ILeaderboardService>();
            mockLeaderboardService.Setup(x => x.UpdateLeaderboardEntry(It.IsAny<long>(), It.IsAny<LeaderboardEntryDTO>()));
            
            var leaderboardEntriesController = new LeaderboardEntriesController(mockLeaderboardContext.Object, mockLeaderboardService.Object);

            // Act
            var result = await leaderboardEntriesController.UpdateLeaderboardEntry(1, leaderboardEntryDTO);

            // Assert
            mockLeaderboardService.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task LeaderboardController_UpdateLeaderboardEntry_BadRequest()
        {
            // Arrange
            var mockLeaderboardService = new Mock<ILeaderboardService>();
            mockLeaderboardService.Setup(x => x.UpdateLeaderboardEntry(It.IsAny<long>(), It.IsAny<LeaderboardEntryDTO>())).Throws(new LeaderboardServiceBadRequestException());

            var leaderboardEntriesController = new LeaderboardEntriesController(mockLeaderboardContext.Object, mockLeaderboardService.Object);

            // Act
            var result = await leaderboardEntriesController.UpdateLeaderboardEntry(1, leaderboardEntryDTO);

            // Assert
            mockLeaderboardService.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task LeaderboardController_UpdateLeaderboardEntry_NotFound()
        {
            // Arrange
            var mockLeaderboardService = new Mock<ILeaderboardService>();
            mockLeaderboardService.Setup(x => x.UpdateLeaderboardEntry(It.IsAny<long>(), It.IsAny<LeaderboardEntryDTO>())).Throws(new LeaderboardServiceNotFoundException());

            var leaderboardEntriesController = new LeaderboardEntriesController(mockLeaderboardContext.Object, mockLeaderboardService.Object);

            // Act
            var result = await leaderboardEntriesController.UpdateLeaderboardEntry(1, leaderboardEntryDTO);

            // Assert
            mockLeaderboardService.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task LeaderboardController_PostNewScore_Valid()
        {
            // Arrange
            var mockLeaderboardService = new Mock<ILeaderboardService>();
            mockLeaderboardService.Setup(x => x.PostNewScore(It.IsAny<NewScoreDTO>()));

            var leaderboardEntriesController = new LeaderboardEntriesController(mockLeaderboardContext.Object, mockLeaderboardService.Object);

            // Act
            var result = await leaderboardEntriesController.PostNewScore(newScoreDTO);

            // Assert
            mockLeaderboardService.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task LeaderboardController_PostNewScore_BadRequest()
        {
            // Arrange
            var mockLeaderboardService = new Mock<ILeaderboardService>();
            mockLeaderboardService.Setup(x => x.PostNewScore(It.IsAny<NewScoreDTO>())).Throws(new LeaderboardServiceBadRequestException());

            var leaderboardEntriesController = new LeaderboardEntriesController(mockLeaderboardContext.Object, mockLeaderboardService.Object);

            // Act
            var result = await leaderboardEntriesController.PostNewScore(newScoreDTO);

            // Assert
            mockLeaderboardService.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task LeaderboardController_DeleteLeaderboardEntry_Valid()
        {
            // Arrange
            var mockLeaderboardService = new Mock<ILeaderboardService>();
            mockLeaderboardService.Setup(x => x.DeleteLeaderboardEntry(It.IsAny<long>()));

            var leaderboardEntriesController = new LeaderboardEntriesController(mockLeaderboardContext.Object, mockLeaderboardService.Object);

            // Act
            var result = await leaderboardEntriesController.DeleteLeaderboardEntry(1);

            // Assert
            mockLeaderboardService.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task LeaderboardController_DeleteLeaderboardEntry_NotFound()
        {
            // Arrange
            var mockLeaderboardService = new Mock<ILeaderboardService>();
            mockLeaderboardService.Setup(x => x.DeleteLeaderboardEntry(It.IsAny<long>())).Throws(new LeaderboardServiceNotFoundException());

            var leaderboardEntriesController = new LeaderboardEntriesController(mockLeaderboardContext.Object, mockLeaderboardService.Object);

            // Act
            var result = await leaderboardEntriesController.DeleteLeaderboardEntry(1);

            // Assert
            mockLeaderboardService.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}