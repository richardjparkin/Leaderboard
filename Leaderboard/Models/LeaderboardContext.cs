using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Models
{
    public class LeaderboardContext : DbContext
    {       public LeaderboardContext()
        {

        }
        public LeaderboardContext(DbContextOptions<LeaderboardContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Player>().HasData(
            new { Id = 1L, FirstName = "Richard", LastName = "Parkin", Email = "richard.parkin@email.co.uk" },
            new { Id = 2L, FirstName = "Adam", LastName = "Smith", Email = "adam.smith@email.co.uk" },
            new { Id = 3L, FirstName = "Jonathan", LastName = "Rhodes", Email = "jonathan.rhodes@email.co.uk" },
            new { Id = 4L, FirstName = "Sylvia", LastName = "Sansom", Email = "sylvia.sansom@email.co.uk" },
            new { Id = 5L, FirstName = "Alex", LastName = "Westby", Email = "alex.westby@email.co.uk" },
            new { Id = 6L, FirstName = "Matthew", LastName = "Hull", Email = "richardparkin@email.co.uk" },
            new { Id = 7L, FirstName = "David", LastName = "Jones", Email = "david.jones@email.co.uk" },
            new { Id = 8L, FirstName = "Jimmy", LastName = "Rigley", Email = "jimmy.rigley@email.co.uk" },
            new { Id = 9L, FirstName = "Judge", LastName = "Judy", Email = "judge.judy@email.co.uk" },
            new { Id = 10L, FirstName = "Pete", LastName = "Castle", Email = "pete.castle@email.co.uk" },
            new { Id = 11L, FirstName = "Barry", LastName = "Scott", Email = "barry.scott@email.co.uk" });

            builder.Entity<Player>(entity => {
                entity.HasIndex(e => e.Email).IsUnique();
            });

            builder.Entity<LeaderboardEntry>().HasData(
            new { Id = 1L, PlayerId = 1L, GamesPlayed = 1L, TotalScore = 100L },
            new { Id = 2L, PlayerId = 2L, GamesPlayed = 2L, TotalScore = 200L },
            new { Id = 3L, PlayerId = 3L, GamesPlayed = 3L, TotalScore = 300L },
            new { Id = 4L, PlayerId = 4L, GamesPlayed = 4L, TotalScore = 400L },
            new { Id = 5L, PlayerId = 5L, GamesPlayed = 5L, TotalScore = 500L },
            new { Id = 6L, PlayerId = 6L, GamesPlayed = 6L, TotalScore = 600L },
            new { Id = 7L, PlayerId = 7L, GamesPlayed = 7L, TotalScore = 700L },
            new { Id = 8L, PlayerId = 8L, GamesPlayed = 8L, TotalScore = 800L },
            new { Id = 9L, PlayerId = 9L, GamesPlayed = 9L, TotalScore = 900L },
            new { Id = 10L, PlayerId = 10L, GamesPlayed = 10L, TotalScore = 1000L });
           
            builder.Entity<LeaderboardEntry>(entity => {
                entity.HasIndex(e => e.PlayerId).IsUnique();
            });
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<LeaderboardEntry> LeaderboardEntries { get; set; }
    }
}