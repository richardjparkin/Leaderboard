using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Models
{
    public class Player
    {
        public long Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public PlayerDTO ToPlayerDTO()
        {
            return new PlayerDTO
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email
            };
        }
    }
}