using Leaderboard.Exceptions;
using Leaderboard.Models;
using Leaderboard.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LeaderboardTests.Services
{
    [TestClass]
    public class LeaderboardServicesTests
    {
        private LeaderboardService leaderboardService;
        private LeaderboardContext leaderboardContext;

        private long playerId = 1;
        private long gamesPlayed = 10;
        private long totalScore = 100;

        [TestInitialize()]
        public void Startup()
        {
            // Simulate DbContext using in memory database
            // Test data is same as LeaderboardContext.OnModelCreating, although should probably be moved to here

            var options = new DbContextOptionsBuilder<LeaderboardContext>()
                .UseInMemoryDatabase("LeaderboardTest")
                .Options;

            leaderboardContext = new LeaderboardContext(options);
            leaderboardService = new LeaderboardService(leaderboardContext);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            leaderboardService = null;
            leaderboardContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void Leaderboard_GetFullLeaderboard_NoLimit()
        {
            // Arrange

            // Act
            var entries = leaderboardService.GetFullLeaderboard();

            // Assert
            Assert.AreEqual(10, entries.Count, "Incorrect number of entries returned");
        }

        [TestMethod]
        public void Leaderboard_GetFullLeaderboard_Limit()
        {
            // Arrange
            int limit = 5;

            // Act
            var entries = leaderboardService.GetFullLeaderboard(limit);

            // Assert
            Assert.AreEqual(5, entries.Count, "Incorrect number of entries returned");
        }

        [TestMethod]
        public void Leaderboard_GetLeaderboardEntry_Valid()
        {
            // Arrange
            long id = 1;

            // Act
            var player = leaderboardService.GetLeaderboardEntry(id);

            // Assert
            Assert.AreEqual(1, player.Id, "Id does not equal {0}", id);
        }

        [TestMethod]
        public void Leaderboard_GetLeaderboardEntry_NotFound()
        {
            // Arrange
            long id = 100;

            // Act

            // Assert
            Assert.ThrowsException<LeaderboardServiceNotFoundException>(() => leaderboardService.GetLeaderboardEntry(id));
        }

        [TestMethod]
        public void Leaderboard_CreateLeaderboardEntry_Valid()
        {
            // Arrange
            LeaderboardEntryDTO leaderboardDTO = new LeaderboardEntryDTO
            {
                PlayerId = 11
            };

            // Act
            var player = leaderboardService.CreateLeaderboardEntry(leaderboardDTO);

            // Assert
            Assert.AreEqual(11, player.Id, "Id does not equal {0}", 11);
         }

        [TestMethod]
        public void Leaderboard_CreateLeaderboardEntry_PlayerIdNotFound()
        {
            // Arrange
            LeaderboardEntryDTO leaderboardDTO = new LeaderboardEntryDTO
            {
                PlayerId = 100
            };

            // Act

            // Assert
            Assert.ThrowsException<LeaderboardServiceBadRequestException>(() => leaderboardService.CreateLeaderboardEntry(leaderboardDTO));
        }


        [TestMethod]
        public void Leaderboard_CreateLeaderboardEntry_PlayerIdUsed()
        {
            // Arrange
            LeaderboardEntryDTO leaderboardDTO = new LeaderboardEntryDTO
            {
                PlayerId = 1
            };

            // Act

            // Assert
            Assert.ThrowsException<LeaderboardServiceBadRequestException>(() => leaderboardService.CreateLeaderboardEntry(leaderboardDTO));
        }

        [TestMethod]
        public void Leaderboard_UpdateLeaderboardEntry_Valid()
        {
            // Arrange
            LeaderboardEntryDTO leaderboardEntryDTO = new LeaderboardEntryDTO
            {
                Id = 1,
                PlayerId = playerId,
                GamesPlayed = gamesPlayed,
                TotalScore = totalScore
            };

            // Act
            leaderboardService.UpdateLeaderboardEntry(1, leaderboardEntryDTO);

            // Assert
            // Framework will catch any thrown exceptions
        }

        [TestMethod]
        public void Leaderboard_UpdateLeaderboardEntry_IdsDontMatch()
        {
            // Arrange
            LeaderboardEntryDTO leaderboardEntryDTO = new LeaderboardEntryDTO
            {
                Id = 2,
                PlayerId = playerId,
                GamesPlayed = gamesPlayed,
                TotalScore = totalScore
            };

            // Act

            // Assert
            Assert.ThrowsException<LeaderboardServiceBadRequestException>(() => leaderboardService.UpdateLeaderboardEntry(1, leaderboardEntryDTO));
        }


        [TestMethod]
        public void Leaderboard_UpdateLeaderboardEntry_NotFound()
        {
            // Arrange
            LeaderboardEntryDTO leaderboardEntryDTO = new LeaderboardEntryDTO
            {
                Id = 100,
                PlayerId = playerId,
                GamesPlayed = gamesPlayed,
                TotalScore = totalScore
            };

            // Act

            // Assert
            Assert.ThrowsException<LeaderboardServiceNotFoundException>(() => leaderboardService.UpdateLeaderboardEntry(100, leaderboardEntryDTO));
        }


        [TestMethod]
        public void Leaderboard_UpdateLeaderboardEntry_PlayerIdNotFound()
        {
            // Arrange
            LeaderboardEntryDTO leaderboardEntryDTO = new LeaderboardEntryDTO
            {
                Id = 1,
                PlayerId = 100,
                GamesPlayed = gamesPlayed,
                TotalScore = totalScore
            };

            // Act

            // Assert
            Assert.ThrowsException<LeaderboardServiceBadRequestException>(() => leaderboardService.UpdateLeaderboardEntry(1, leaderboardEntryDTO));
        }

        [TestMethod]
        public void Leaderboard_UpdateLeaderboardEntry_PlayerIdUsed()
        {
            // Arrange
            LeaderboardEntryDTO leaderboardEntryDTO = new LeaderboardEntryDTO
            {
                Id = 1,
                PlayerId = 2,
                GamesPlayed = gamesPlayed,
                TotalScore = totalScore
            };

            // Act

            // Assert
            Assert.ThrowsException<LeaderboardServiceBadRequestException>(() => leaderboardService.UpdateLeaderboardEntry(1, leaderboardEntryDTO));
        }

        [TestMethod]
        public void Leaderboard_PostNewScore_Valid()
        {
            // Arrange
            NewScoreDTO newScoreDTO = new NewScoreDTO
            {
                PlayerId = 1,
                TotalScore = 100
            };

            // Act
            leaderboardService.PostNewScore(newScoreDTO);

            // Assert
            // Framework will catch any thrown exceptions
        }

        [TestMethod]
        public void Leaderboard_PostNewScore_Valid_PlayerIdNotFound()
        {
            // Arrange
            NewScoreDTO newScoreDTO = new NewScoreDTO
            {
                PlayerId = 100,
                TotalScore = 100
            };

            // Act
           
            // Assert
            Assert.ThrowsException<LeaderboardServiceBadRequestException>(() => leaderboardService.PostNewScore(newScoreDTO));
        }

        [TestMethod]
        public void Leaderboard_DeleteLeaderboardEntry_Valid()
        {
            // Arrange
            long id = 1;

            // Act
            leaderboardService.DeleteLeaderboardEntry(id);

            // Assert
            // Framework will catch any thrown exceptions
        }

        [TestMethod]
        public void Leaderboard_DeleteLeaderboardEntry_NotFound()
        {
            // Arrange
            long id = 100;

            // Act

            // Assert
            Assert.ThrowsException<LeaderboardServiceNotFoundException>(() => leaderboardService.DeleteLeaderboardEntry(id));
        }
    }
}



//public void PostNewScore(NewScoreDTO newScoreDTO)
//public void DeleteLeaderboardEntry(long id)




